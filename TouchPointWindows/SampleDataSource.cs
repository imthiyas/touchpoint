using System.Collections.ObjectModel;
using TouchPointWindows.DataModel;

namespace TouchPointWindows
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            //Groups = new ObservableCollection<MenuGroup>();
            //for (int i = 0; i < 5; i++)
            //{
            //    var menuGroup = new MenuGroup("MainGroup" + i, "", "");
            //    menuGroup.ChildGroups = new ObservableCollection<MenuGroup>();
            //    for (int j = 0; j < 5; j++)
            //    {
            //        var childGroup = new MenuGroup("ChildGroup" + j, "", ""); 
            //        menuGroup.ChildGroups.Add(childGroup);
            //        for (int k = 1; k < 11; k++)
            //        {
            //            var item = new Menu("Item" + k, (decimal) (2.99*k), "", "some desc", "some content");
            //            childGroup.Items.Add(item);
            //        }
            //    }
            //    Groups.Add(menuGroup);
          //  }
        }

        public ObservableCollection<MenuGroup> Groups { get; set; }
    }
}
