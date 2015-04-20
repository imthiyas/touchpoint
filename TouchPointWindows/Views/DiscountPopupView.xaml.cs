using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using TouchPointWindows.Common;

namespace TouchPointWindows.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DiscountPopupView : PageBase
    {
        public DiscountPopupView()
        {
            this.InitializeComponent();
        }

        private void DiscountPopup_OnOpened(object sender, object e)
        {
            //First we need to find out how big our window is, so we can center to it.
            CoreWindow currentWindow = Window.Current.CoreWindow;

            //Now we figure out where the center of the screen is, and we 
            //move the popup to that location.
            discountPopup.HorizontalOffset = -20;
            discountPopup.VerticalOffset = (currentWindow.Bounds.Height / 2) - (150 / 2);

            discountPopupGridWidth.Width = currentWindow.Bounds.Width + 20;
            //Presto!  We have a centered popup.

        }
    }
}
