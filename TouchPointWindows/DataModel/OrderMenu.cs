using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using TouchPointWindows.Annotations;

namespace TouchPointWindows.DataModel
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class OrderMenu : NotifyBase
    {
        private int _quantity;
        private int _index;
        private Menu _item;

        public OrderMenu(Menu item)
        {
            Item = item;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Menu Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }

        public int MenuId
        {
            get { return Item.Id; }
        }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }

        public int Quantity 
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                Item.Quantity = _quantity;
                OnPropertyChanged();
            }
        } 

        public Double Cost               // only used when the order is made
        {
            get
            {
                return Price * Quantity;
            }
        }

        public Double Price   // only used when the order is made
        {
            get { return Item.Price; }
        }
    }
}