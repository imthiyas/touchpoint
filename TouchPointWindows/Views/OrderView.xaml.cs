// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Core;
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
    public partial class OrderView : PageBase
    {
        public OrderView()
        {
            this.InitializeComponent();
        }
    }
}