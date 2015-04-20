using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TouchPointWindows.Annotations;
using TouchPointWindows.DataModel;
using TouchPointWindows.Services;
using TouchPointWindows.ViewModels.Interfaces;

namespace TouchPointWindows.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OrderViewModel : ViewModelBase, IViewModelBase
    {
        private readonly IDataService _dataService;
        private List<MenuGroup> _groups;
        private List<MenuGroup> _groupedItems;
        private ObservableCollection<OrderMenu> _orderMenu;
        private List<MenuModel> _menuModels;
        private double _netValue;
        private double _vatValue;
        private bool _isDiscountPercent;
        private double _discountValueApplied;
        private double _totalValue;
        private MenuGroup _menuGroupSelectedItem;
        private bool _isDiscountPopup;
        private double _discountPercent;
        private double _discountOnTotalValue;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public OrderViewModel(IDataService dataService)
        {
            _dataService = dataService;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            MenuItemClickCommand = new RelayCommand<ItemClickEventArgs>(MenuItemClickCommandExecute);
            RemoveItemCommand = new RelayCommand<object>(RemoveItemCommandExecute);
            DiscountPopupCommand = new RelayCommand(DiscountPopupCommandExecute);
            Messenger.Default.Register<DiscountModel>(this, OnDiscountModelReceived);
        }

        private void OnDiscountModelReceived(DiscountModel discountModel)
        {
            if (discountModel.IsDiscountPercent)
            {
                DiscountValueApplied = (discountModel.DiscountPercent / 100) * TotalValue;
            }
            else
            {
                DiscountValueApplied = discountModel.DiscountOnTotalValue;
            }
            CalculateBottomValues();
        }

        private void DiscountPopupCommandExecute()
        {
            IsDiscountPopup = true;
            var dicountModel = new DiscountModel()
            {
                DiscountOnTotalValue = CurrentOrder.DiscountModel.DiscountOnTotalValue, 
                DiscountPercent = CurrentOrder.DiscountModel.DiscountPercent
            };
            Messenger.Default.Send<DiscountModel>(dicountModel);
        }

        private void RemoveItemCommandExecute(object objId)
        {
            var id = Convert.ToInt16(objId);
            CurrentOrder.RemoveItem(id);
            CalculateBottomValues();
        }

        private void MenuItemClickCommandExecute(ItemClickEventArgs e)
        {
            var name = ((Menu)e.ClickedItem).Name;
            var menu = (Menu)e.ClickedItem;
            var item = new OrderMenu(menu);

            CurrentOrder.AddItem(item);

            //OrderMenu = null;
            OrderMenu = CurrentOrder.Items;
            CalculateBottomValues();
        }

        private void CalculateBottomValues()
        {
            CalculateNetValue();
            CalculateTotalValue();
        }

        private void CalculateNetValue()
        {
            var menus = OrderMenu.Select(c => c.Item).ToList();
            NetValue = menus.Sum(c => c.Price * c.Quantity);
            VatValue = menus.Sum(c => (c.Vat / 100) * NetValue);
        }

        private void CalculateTotalValue()
        {
            TotalValue = NetValue + VatValue - DiscountValueApplied;
        }

        private async void LoadView()
        {
            Orders = new List<Order>();
            CurrentOrder = new Order();
             CurrentOrder.DiscountModel = new DiscountModel();
            Orders.Add(CurrentOrder);

            Groups = await _dataService.GetAllDbItemsAsync();
            MenuGroupSelectedItem = Groups.First();
        }

        public List<Order> Orders { get; set; }

        public Order CurrentOrder { get; set; }
        public List<MenuGroup> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                RaisePropertyChanged();
            }
        }

        public List<MenuGroup> GroupedItems
        {
            get { return _groupedItems; }
            set { _groupedItems = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<OrderMenu> OrderMenu
        {
            get { return _orderMenu; }
            set { _orderMenu = value; RaisePropertyChanged(); }
        }

        public List<MenuModel> MenuModels
        {
            get { return _menuModels; }
            set { _menuModels = value; RaisePropertyChanged(); }
        }

        public MenuGroup MenuGroupSelectedItem
        {
            get { return _menuGroupSelectedItem; }
            set
            {
                _menuGroupSelectedItem = value;
                GroupedItems = _menuGroupSelectedItem.ChildGroups;
                RaisePropertyChanged();
            }
        }

        public Double NetValue
        {
            get { return _netValue; }
            set { _netValue = value; RaisePropertyChanged(); }
        }

        public Double VatValue
        {
            get { return _vatValue; }
            set { _vatValue = value; RaisePropertyChanged(); }
        }

        public Double DiscountValueApplied
        {
            get { return _discountValueApplied; }
            set { _discountValueApplied = value; RaisePropertyChanged(); }
        }

        public Double TotalValue
        {
            get { return _totalValue; }
            set { _totalValue = value; RaisePropertyChanged(); }
        }

        public bool IsDiscountPopup
        {
            get { return _isDiscountPopup; }
            set { _isDiscountPopup = value; RaisePropertyChanged(); }
        }

        public RelayCommand<ItemClickEventArgs> MenuItemClickCommand { get; set; }
        public RelayCommand<object> RemoveItemCommand { get; set; }
        public RelayCommand DiscountPopupCommand { get; set; }


        public void Load()
        {
            LoadView();
        }
    }
}