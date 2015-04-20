using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using TouchPointWindows.Common;
using TouchPointWindows.ViewModels.Interfaces;
using TouchPointWindows.Views;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace TouchPointWindows.ViewModels
{
    public class LoginViewModel : ViewModelBase, IViewModelBase
    {
        private string _buttonText;

        public LoginViewModel()
        {
            AddMenuItemCommand = new RelayCommand(AddMenuItemCommandExecute);
            TakeOrderCommand = new RelayCommand(TakeOrderCommandExecute);
        }

        private void TakeOrderCommandExecute()
        {
            var content = Window.Current.Content;
            var frame = content as Frame;

            if (frame != null)
            {
                frame.Navigate(typeof(OrderView));
            }
            Window.Current.Activate();   
        }

        private void AddMenuItemCommandExecute()
        {
            var content = Window.Current.Content;
            var frame = content as Frame;

            if (frame != null)
            {
                frame.Navigate(typeof(MenuView));
            }
            Window.Current.Activate(); 
        }

        public RelayCommand AddMenuItemCommand { get; set; }
        public RelayCommand TakeOrderCommand { get; set; }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; RaisePropertyChanged();}
        }

        public void Load()
        {
            ButtonText = "Test";
        }
    }
}
