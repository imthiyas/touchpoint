// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Telerik.Storage.Extensions;
using TouchPointWindows.DataModel;

namespace TouchPointWindows.Services
{
    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// </summary>
    public class DataService : IDataService
    {
        private static int _index;
        private static Context _context;

        public DataService()
        {
            _context = new Context("TouchPointDB10", DatabaseLocation.Local);
        }

        public async Task<MenuGroup> GetFolderGroupsAsync()
        {
            var mainGroup = await GetSampleDataAsync();
            return mainGroup;
        }

        private async Task<List<MenuGroup>> GetDbGroupsAsync(int parentId)
        {
            var menuGroups = await _context.GetAsync<MenuGroup>("SELECT * FROM MenuGroup WHERE ParentId=" + parentId);
            return menuGroups;
        }

        //private async Task GetAllFolderItemsOld(MenuGroup parentGroup, StorageFolder storageFolder)
        //{
        //    var folders = await storageFolder.GetFoldersAsync();
        //    foreach (StorageFolder folder in folders)
        //    {
        //        var childGroup = new MenuGroup(folder.Name, "", "");
        //        var files = await folder.GetFilesAsync();
        //        foreach (StorageFile file in files)
        //        {
        //            decimal price = 0;
        //            var name = file.Name.Replace(".jpg", String.Empty);
        //            var regxValue = Regex.Match(name, @"\d+(\.\d+)?");
        //            if (regxValue.Success)
        //            {
        //                price = decimal.Parse(regxValue.Value, CultureInfo.InvariantCulture);
        //            }
        //            name = name.Replace(regxValue.Value, String.Empty).TrimEnd(new[] { '-' });
        //            decimal vat = 20;
        //            childGroup.Items.Add(new Menu(++_index, name, price, vat, file.Path, "", ""));
        //        }
        //        parentGroup.ChildGroups.Add(childGroup);
        //        await GetAllFolderItems(childGroup, folder);
        //    }
        //}

        private async Task GetAllFolderItems(MenuGroup parentGroup, StorageFolder storageFolder)
        {
            var folders = await storageFolder.GetFoldersAsync();
            foreach (StorageFolder folder in folders)
            {
                var childGroup = new MenuGroup(folder.Name, parentGroup.Name);
                var files = await folder.GetFilesAsync();
                foreach (StorageFile file in files)
                {
                    decimal price = 0;
                    var name = file.Name.Replace(".jpg", String.Empty);
                    var regxValue = Regex.Match(name, @"\d+(\.\d+)?");
                    if (regxValue.Success)
                    {
                        price = decimal.Parse(regxValue.Value, CultureInfo.InvariantCulture);
                    }
                    name = name.Replace(regxValue.Value, String.Empty).TrimEnd(new[] { '-' });
                    var newMenu = new Menu(name, file.Path, childGroup.Name, parentGroup.Name);
                    childGroup.Items.Add(newMenu);
                }
                parentGroup.ChildGroups.Add(childGroup);
                await GetAllFolderItems(childGroup, folder);
            }
        }

        private async Task<MenuGroup> GetSampleDataAsync()
        {
            var root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            var path = root + @"\Assets";

            // Get the folder object that corresponds to
            // this absolute path in the file system.
            var folder = await StorageFolder.GetFolderFromPathAsync(path);
            var imagesFolder = await folder.GetFolderAsync("images");

            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //var touchPointFolder = await KnownFolders.PicturesLibrary.GetFolderAsync("TouchPoint");
            //var imagesFolder = await touchPointFolder.GetFolderAsync("images");
            var mainGroup = new MenuGroup("Main", "");
            await GetAllFolderItems(mainGroup, imagesFolder);
            return mainGroup;
        }

        //public async void SaveMenuItem(IEnumerable<MenuGroup> itemGroup)
        //{
        //    foreach (var menuGroup in itemGroup)
        //    {
        //        var groupFound = _context.GetAll<MenuGroup>().FirstOrDefault(c => c.Name == menuGroup.Name);
        //        if (groupFound != null)
        //        {
        //            _context.Insert<MenuGroup>(menuGroup);
        //        }
        //        foreach (var item in menuGroup.Items)
        //        {
        //            var itemFound = _context.GetAll<Menu>().FirstOrDefault(c => c.Name == item.Name);
        //            if (itemFound != null)
        //            {
        //                _context.Insert<Menu>(item);
        //            }
        //        }
        //    }
        //    _context.SaveChanges();
        //}     


        public async Task SaveMenuItemsAsync(IEnumerable<Menu> items)
        {
            foreach (var item in items)
            {
                _context.Insert<Menu>(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<MenuGroup>> GetAllDbItemsAsync()
        {
            var menuGroups = await _context.GetAsync<MenuGroup>("SELECT * FROM MenuGroup WHERE ParentId = 0");
            foreach (var menuGroup in menuGroups)
            {
                var childGroups = await _context.GetAsync<MenuGroup>("SELECT * FROM MenuGroup WHERE ParentId = " + menuGroup.Id);
                foreach (var group in childGroups)
                {
                    group.Items = await _context.GetAsync<Menu>("SELECT * FROM Menu WHERE ParentGroupId = " + group.Id);
                }
                menuGroup.ChildGroups = childGroups;
            }
            return menuGroups;
        }

        public async Task<List<MenuGroup>> SaveMenuGroupsAsync(IEnumerable<MenuGroup> groups, int parentId)
        {
            if (parentId == 0)
            {
                _context.GetScalar<int>("DELETE FROM MenuGroup");
                _context.GetScalar<int>("DELETE FROM Menu");
            }
            foreach (var menuGroup in groups)
            {
                _context.Insert<MenuGroup>(menuGroup);
            }
            await _context.SaveChangesAsync();
            return await GetDbGroupsAsync(parentId);
        }

        public async Task<ObservableCollection<MenuModel>> GetAllMenuModelAsync()
        {
            var menuGroup = await GetFolderGroupsAsync();
            var childGroups = menuGroup.ChildGroups.SelectMany(c => c.ChildGroups);
            var mainGroups = menuGroup.ChildGroups;
            var menuGroups = childGroups as MenuGroup[] ?? childGroups.ToArray();
            var menus = menuGroups.SelectMany(c => c.Items);

            var allFolderMenus = from item in menus
                                 join child in menuGroups on item.ParentGroupName equals child.Name
                                 join grp in mainGroups on child.ParentName equals grp.Name
                                 where item.GrandParentGroupName == grp.Name
                                 select new MenuModel
                                 {
                                     GroupName = grp.Name,
                                     ChildName = child.Name,
                                     Name = item.Name,
                                     Description = item.Description,
                                     Price = item.Price,
                                     Vat = item.Vat,
                                     ImagePath = item.ImagePath
                                 };


            var dbmainGroups = await _context.GetAsync<MenuGroup>("SELECT * FROM MenuGroup WHERE ParentId = 0");
            var dbchildGroups = await _context.GetAsync<MenuGroup>("SELECT * FROM MenuGroup WHERE ParentId > 0");
            var dbmenus = await _context.GetAsync<Menu>("SELECT * FROM Menu");

            var allDbMenus = from item in dbmenus
                             join child in dbchildGroups on item.ParentGroupId equals child.Id
                             join grp in dbmainGroups on child.ParentId equals grp.Id
                             select new MenuModel
                             {
                                 GroupName = grp.Name,
                                 ChildName = child.Name,
                                 Name = item.Name,
                                 Description = item.Description,
                                 Price = item.Price,
                                 Vat = item.Vat,
                                 ImagePath = item.ImagePath
                             };

            var combined = allDbMenus.ToList();

            var newFolderMenus = from folderMenu in allFolderMenus
                                 join dbMenu in combined on new
                                 {
                                     p1 = folderMenu.GroupName,
                                     p2 = folderMenu.ChildName,
                                     p3 = folderMenu.Name,
                                 } equals new
                                 {
                                     p1 = dbMenu.GroupName,
                                     p2 = dbMenu.ChildName,
                                     p3 = dbMenu.Name,
                                 } into df
                                 from joined in df.DefaultIfEmpty()
                                 select joined == null ? folderMenu : null;

            var menuModels = newFolderMenus.OfType<MenuModel>().ToList();
            combined.AddRange(menuModels);

            var combinedObservableCollection = new ObservableCollection<MenuModel>(combined);
            return combinedObservableCollection;
        }
    }
}