using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TouchPointWindows.ViewModels;

namespace TouchPointWindows.DataModel
{
    public class Order: NotifyBase
    {
        private int _index;
        private bool _isDiscountPercent;
        private bool _isDiscountPercent1;
        private double _discountPercent;
        private double _discountOnTotalValue;
        private DiscountModel _discountModel;

        public Order()
        {
            Items = new ObservableCollection<OrderMenu>();
            CustomerId = 1;
        }
        public Order(Customer customer)
        {
            this.CustomerId = customer.Id;
            Items = new ObservableCollection<OrderMenu>();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public ObservableCollection<OrderMenu> Items { get; set; }

        public void AddItem(OrderMenu item)
        {
            var itemFound = Items.FirstOrDefault(c => c.MenuId == item.MenuId);
            if (itemFound != null)
            {
                itemFound.Quantity++;
            }
            else
            {
                item.Quantity = 1;
                item.Index = ++_index;
                Items.Add(item);
            }
        }

        public void RemoveItem(int menuId)
        {
            var itemFound = Items.FirstOrDefault(c => c.MenuId == menuId);
            if (itemFound != null)
            {
                if (--itemFound.Quantity == 0)
                {
                    var emptyItemIndex = Items.IndexOf(itemFound);
                    for (int i = emptyItemIndex + 1; i <= Items.Count - 1; i++)
                    {
                        Items[i].Index = i;
                    }
                    _index = Items.Count - 1;
                    Items.Remove(itemFound);
                }
            }
        }

        public DiscountModel DiscountModel
        {
            get { return _discountModel; }
            set
            {
                if (Equals(value, _discountModel)) return;
                _discountModel = value;
                OnPropertyChanged();
            }
        }
    }
}





