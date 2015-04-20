using System;
using System.Runtime.CompilerServices;
using TouchPointWindows.DataModel;

namespace TouchPointWindows.ViewModels
{
    public class DiscountModel : NotifyBase
    {
        private double _discountPercent;
        private double _discountOnTotalValue;
        private bool _isDiscountPercent;
        private double _totalValue;

        private void MakeOtherDiscountInValid([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "DiscountPercent")
            {
                DiscountOnTotalValue = 0;
                IsDiscountPercent = true;
            }
            else
            {
                DiscountPercent = 0;
                IsDiscountPercent = false;
            }
        }

        public double TotalValue
        {
            get { return _totalValue; }
            set
            {
                if (value.Equals(_totalValue)) return;
                _totalValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsDiscountPercent
        {
            get { return _isDiscountPercent; }
            set
            {
                if (value.Equals(_isDiscountPercent)) return;
                _isDiscountPercent = value;
                OnPropertyChanged();
            }
        }

        public Double DiscountPercent
        {
            get { return _discountPercent; }
            set
            {
                if (value != 0)
                {
                    MakeOtherDiscountInValid();
                } //this method is called so that the other discount is invalid and only one type of discount is allowed at a time
                _discountPercent = value;
                OnPropertyChanged();
            }
        }

        public Double DiscountOnTotalValue
        {
            get { return _discountOnTotalValue; }
            set
            {
                if (value != 0)
                {
                    MakeOtherDiscountInValid();
                }
                _discountOnTotalValue = value;
                OnPropertyChanged();
            }
        }
    }
}