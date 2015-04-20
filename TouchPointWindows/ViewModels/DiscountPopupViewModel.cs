using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TouchPointWindows.ViewModels.Interfaces;

namespace TouchPointWindows.ViewModels
{
    public class DiscountPopupViewModel : ViewModelBase, IViewModelBase
    {
        private DiscountModel _discountModel;
        private bool _isDiscountPopup;

        public DiscountPopupViewModel()
        {
            Messenger.Default.Register<DiscountModel>(this, OnDiscountPopupOpen);
            CloseDiscountPopupCommand = new RelayCommand(CloseDiscountPopupCommandExecute);
            ApplyDiscountCommand = new RelayCommand(ApplyDiscountCommandExecute);
        }

        private void CloseDiscountPopupCommandExecute()
        {
            Messenger.Default.Send<DiscountModel>(OldDiscountModel);
            IsDiscountPopup = false;
        }

        private void ApplyDiscountCommandExecute()
        {
            Messenger.Default.Send<DiscountModel>(DiscountModel);
            IsDiscountPopup = false;
        }

        private void OnDiscountPopupOpen(DiscountModel discountModel)
        {
            DiscountModel = discountModel;
            OldDiscountModel = new DiscountModel //this is created in case cancel is clicked after changing the value
            {
                DiscountOnTotalValue = discountModel.DiscountOnTotalValue,
                DiscountPercent = discountModel.DiscountPercent,
                TotalValue = discountModel.TotalValue,
                IsDiscountPercent = discountModel.DiscountPercent > 0
            }; 
        }

        public DiscountModel OldDiscountModel { get; set; }
        public DiscountModel DiscountModel
        {
            get { return _discountModel; }
            set { _discountModel = value; RaisePropertyChanged(); }
        }

        public bool IsDiscountPopup
        {
            get { return _isDiscountPopup; }
            set { _isDiscountPopup = value; RaisePropertyChanged(); }
        }

        public RelayCommand ApplyDiscountCommand { get; set; }
        public RelayCommand CloseDiscountPopupCommand { get; set; }

        public void Load()
        {
            
        }
    }
}
