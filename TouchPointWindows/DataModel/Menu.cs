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
    public class Menu : NotifyBase
    {
        private int _quantity;
        private Visibility _isOrdered;
        private Double _vat;
        private Double _price;
        private string _description;

        public Menu() {}

        public Menu(string name, string imagePath, string parentGroupName, string grandParentGroupName)
        {
            this.Name = name;
            this.ImagePath = imagePath;
            ParentGroupName = parentGroupName;
            GrandParentGroupName = grandParentGroupName;
            IsOrdered = Visibility.Collapsed;
        }
        public Menu(string name, Double vat, Double price, string description, string imagePath, int parentGroupId)
        {
            this.Name = name;
            Vat = vat;
            Price = price;
            Description = description;
            ImagePath = imagePath;
            ParentGroupId = parentGroupId;
            IsOrdered = Visibility.Collapsed;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ParentGroupId { get; set; }
        public string ParentGroupName { get; set; }
        public string GrandParentGroupName { get; set; }
        public string Name { get; set; }

        public Double Vat
        {
            get { return _vat; }
            set
            {
                _vat = value;
                OnPropertyChanged();
            }
        } //this is VAT percentage applied on price

        public Double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath { get; set; }
        public string Content { get; set; }

        public int Quantity // only used when the order is made - display purpose
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                IsOrdered = _quantity > 0 ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged();
            }
        } 

        public Visibility IsOrdered
        {
            get { return _isOrdered; }
            set
            {
                _isOrdered = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}