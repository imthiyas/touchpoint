// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using TouchPointWindows.Common;
using TouchPointWindows.DataModel;

namespace TouchPointWindows.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class OrderView1
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private MenuGroup _mainGroup;
        private ObservableCollection<Order> _orderedItems = new ObservableCollection<Order>();
        private Customer _customer = new Customer();
        private Order _order;
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ICommand RemoveItemCommand { get; set; }

        public OrderView1()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            //RemoveItemCommand = new RelayCommand(OnRemoveItemCommandExecute);
            //SetSampleValue();
        }

        private void OnRemoveItemCommandExecute()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            _mainGroup = await DataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = _mainGroup.ChildGroups;
            this.DefaultViewModel["GroupedItems"] = _mainGroup.ChildGroups.First().ChildGroups;
            _order = new Order(_customer);
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter

            // this.Frame.Navigate(typeof(GroupDetailPage), ((MenuGroup)group).Name);
        }
        void Group_Click(object sender, SelectionChangedEventArgs e)
        {
            // Determine what group the Button instance represents
            var menuGroup = (MenuGroup)e.AddedItems[0];
            this.DefaultViewModel["GroupedItems"] = menuGroup.ChildGroups;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter

            //this.Frame.Navigate(typeof(GroupDetailPage), ((MenuGroup)group).Name);
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var name = ((Menu)e.ClickedItem).Name;
            var menu = (Menu)e.ClickedItem;
            var item = new OrderMenu(menu);
            _order.AddItem(item);

            this.DefaultViewModel["Order"] = null;
            this.DefaultViewModel["Order"] = _order.Items;
            //this.Frame.Navigate(typeof(ItemDetailPage), name);
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void RemoveItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            var removeButton = sender as Button;
            var id = Convert.ToInt16(removeButton.CommandParameter);
            _order.RemoveItem(id);
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}