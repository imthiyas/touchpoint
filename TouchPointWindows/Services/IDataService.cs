using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TouchPointWindows.Annotations;
using TouchPointWindows.DataModel;

namespace TouchPointWindows.Services
{
    public interface IDataService
    {
        Task<MenuGroup> GetFolderGroupsAsync();
        Task<ObservableCollection<MenuModel>> GetAllMenuModelAsync();
        Task<List<MenuGroup>> SaveMenuGroupsAsync(IEnumerable<MenuGroup> menuGroups, int parentId);
        Task SaveMenuItemsAsync(IEnumerable<Menu> menuItems);
        Task<List<MenuGroup>> GetAllDbItemsAsync();
    }
}
    