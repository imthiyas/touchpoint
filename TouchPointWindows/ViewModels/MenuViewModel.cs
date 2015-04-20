using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using TouchPointWindows.Common;
using TouchPointWindows.DataModel;
using TouchPointWindows.Services;
using TouchPointWindows.ViewModels.Interfaces;

namespace TouchPointWindows.ViewModels
{
    public class MenuViewModel : ViewModelBase, IViewModelBase
    {
        private readonly IDataService _dataService;
        private ObservableCollection<MenuModel> _menuModelItems;

        public MenuViewModel(IDataService dataService )
        {
            _dataService = dataService;
            SaveCommand = new RelayCommand(SaveCommandExecute);
        }

        private async void SaveCommandExecute()
        {
            var topGroups = await SaveAndGetTopGroups();
            foreach (var menuGroup in topGroups)
            {
                var middleGroups = await SaveAndGetMiddleGroups(menuGroup);
                foreach (var middleGroup in middleGroups)
                {
                    await SaveMenus(middleGroup, menuGroup.Name);
                }
            }
        }

        private async Task<List<MenuGroup>> SaveAndGetTopGroups()
        {
            var groups = new List<MenuGroup>();
            foreach (var groupName in MenuModelItems.Select(c=>c.GroupName).Distinct())
            {
                var group = new MenuGroup(groupName, 0);
                groups.Add(group);
            }
            return await _dataService.SaveMenuGroupsAsync(groups, 0);
        }

        private async Task<List<MenuGroup>> SaveAndGetMiddleGroups(MenuGroup topGroup)
        {
            var groups = new List<MenuGroup>();
            var distictChildNames = MenuModelItems.Where(c => c.GroupName == topGroup.Name).
                                                  Select(c => c.ChildName).Distinct();
            foreach (var childName in distictChildNames)
            {
                var group = new MenuGroup(childName, topGroup.Id);
                groups.Add(group);
            }
            return await _dataService.SaveMenuGroupsAsync(groups, topGroup.Id);
        }
        private async Task SaveMenus(MenuGroup middleGroup, string parentName)
        {
            var menus = new List<Menu>();
            foreach (var menuModel in MenuModelItems.Where(c => c.GroupName == parentName && c.ChildName == middleGroup.Name))
            {
                var menu = new Menu(menuModel.Name, menuModel.Vat, menuModel.Price, menuModel.Description, menuModel.ImagePath, middleGroup.Id);
                menus.Add(menu);
            }
            await _dataService.SaveMenuItemsAsync(menus);
        }

        private async void LoadItems()
        {
            MenuModelItems = await _dataService.GetAllMenuModelAsync();
        }

        public RelayCommand SaveCommand { get; set; }
        public ObservableCollection<MenuModel> MenuModelItems
        {
            get { return _menuModelItems; }
            set { _menuModelItems = value; RaisePropertyChanged(); }
        }

        public void Load()
        {
            LoadItems();
        }
    }
}
