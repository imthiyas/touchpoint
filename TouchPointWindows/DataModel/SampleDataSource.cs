using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace TouchPointWindows.Data
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class Menu
    {
        public Menu(String name, decimal price, String imagePath, String description, String content)
        {
            this.Name = name;
            this.Price = price;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
        }

        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class MenuGroup
    {
        public MenuGroup(String name, String imagePath, String description)
        {
            this.Name = name;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<Menu>();
            this.ChildGroups = new ObservableCollection<MenuGroup>();
        }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<Menu> Items { get; private set; }
        public ObservableCollection<MenuGroup> ChildGroups { get; private set; }
        public override string ToString()
        {
            return this.Name;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// 
    /// SampleDataSource initializes with data read from a static json file included in the 
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class MenuSource
    {
        private static MenuSource _sampleDataSource = new MenuSource();

        private ObservableCollection<MenuGroup> _groups = new ObservableCollection<MenuGroup>();
        public ObservableCollection<MenuGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<MenuGroup> GetGroupsAsync()
        {
            var mainGroup = await _sampleDataSource.GetSampleDataAsync();

            return mainGroup;
        }

        public static async Task<MenuGroup> GetGroupAsync(string name)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groups.Where((group) => group.Name.Equals(name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<Menu> GetItemAsync(string name)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.Name.Equals(name));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        // This is the method to convert the StorageFile to a Byte[] 
        public async Task<Image> GetImageAsync(StorageFile storageFile)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await storageFile.OpenAsync(FileAccessMode.Read);
            bitmapImage.SetSource(stream);
            Image image = new Image();
            image.Source = bitmapImage;
            return image;
        }

        private async Task GetAllFolderItems(MenuGroup parentGroup, StorageFolder storageFolder)
        {
            var folders = await storageFolder.GetFoldersAsync();
            for (int j = 0; j < folders.Count; j++)
            {
                var childGroup = new MenuGroup(folders[j].Name, "", "");
                var files = await folders[j].GetFilesAsync();
                for (int i = 0; i < files.Count; i++)
                {
                    decimal price = 0;
                    var name = files[i].Name.Replace(".jpg", String.Empty);
                    var regxValue = Regex.Match(name, @"\d+(\.\d+)?");
                    if (regxValue.Success)
                    {
                        price = decimal.Parse(regxValue.Value, CultureInfo.InvariantCulture);
                    }
                    name = name.Replace(regxValue.Value, String.Empty);
                    childGroup.Items.Add(new Menu(files[i].Name, price, files[i].Path, "", ""));
                }
                parentGroup.ChildGroups.Add(childGroup);
                await GetAllFolderItems(childGroup, folders[j]);
            }
        }

        private async Task<MenuGroup> GetSampleDataAsync()
        {
            var touchPointFolder = await KnownFolders.PicturesLibrary.GetFolderAsync("TouchPoint");
            var imagesFolder = await touchPointFolder.GetFolderAsync("images");
            var mainGroup = new MenuGroup("Main", "", "");
            await GetAllFolderItems(mainGroup, imagesFolder);
            return mainGroup;
        }
    }
}