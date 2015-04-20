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
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace TouchPointWindows.DataModel
{
    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// </summary>
    public sealed class DataSource
    {
        private static readonly DataSource _dataSource = new DataSource();

        private readonly ObservableCollection<MenuGroup> _groups = new ObservableCollection<MenuGroup>();
        public ObservableCollection<MenuGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<MenuGroup> GetGroupsAsync()
        {
            var mainGroup = await _dataSource.GetSampleDataAsync();
            return mainGroup;
        }

        public static async Task<MenuGroup> GetGroupAsync(string name)
        {
            await _dataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _dataSource.Groups.Where((group) => group.Name.Equals(name));
            var menuGroups = matches as IList<MenuGroup> ?? matches.ToList();
            return menuGroups.Count() == 1 ? menuGroups.First() : null;
        }

        public static async Task<Menu> GetItemAsync(string name)
        {
            await _dataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _dataSource.Groups.SelectMany(group => group.Items).Where((item) => item.Name.Equals(name));
            var enumerable = matches as IList<Menu> ?? matches.ToList();
            if (enumerable.Count() == 1) return enumerable.First();
            return null;
        }

        // This is the method to convert the StorageFile to a Byte[] 
        public async Task<Image> GetImageAsync(StorageFile storageFile)
        {
            var bitmapImage = new BitmapImage();
            var stream = (FileRandomAccessStream)await storageFile.OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);
            var image = new Image {Source = bitmapImage};
            return image;
        }

        private async Task GetAllFolderItems(MenuGroup parentGroup, StorageFolder storageFolder)
        {
            var folders = await storageFolder.GetFoldersAsync();
            foreach (StorageFolder folder in folders)
            {
                var childGroup = new MenuGroup(folder.Name,"", "", "");
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
                    name = name.Replace(regxValue.Value, String.Empty);
                    childGroup.Items.Add(new Menu(1, file.Name, price, 20, file.Path, "", ""));
                }
                parentGroup.ChildGroups.Add(childGroup);
                await GetAllFolderItems(childGroup, folder);
            }
        }

        private async Task<MenuGroup> GetSampleDataAsync()
        {
            string root =
                Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            string path = root + @"\Assets";

            // Get the folder object that corresponds to
            // this absolute path in the file system.
            StorageFolder folder =
                await StorageFolder.GetFolderFromPathAsync(path);
            var imagesFolder = await folder.GetFolderAsync("images");

            //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //var touchPointFolder = await KnownFolders.PicturesLibrary.GetFolderAsync("TouchPoint");
            //var imagesFolder = await touchPointFolder.GetFolderAsync("images");
            var mainGroup = new MenuGroup("Main", "", "");
            await GetAllFolderItems(mainGroup, imagesFolder);
            return mainGroup;
        }
    }
}