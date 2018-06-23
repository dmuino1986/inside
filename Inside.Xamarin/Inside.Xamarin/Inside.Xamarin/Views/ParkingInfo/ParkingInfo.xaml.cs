using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inside.Xamarin.Views.ParkingInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingInfo : ContentPage
    {
        public ParkingInfo()
        {
            InitializeComponent();
        }

        void MainPageSizeChanged(object sender, EventArgs e)
        {
            if (App.Current.MainPage.Width > App.Current.MainPage.Height)
                mainContent.Orientation = StackOrientation.Horizontal;
            else
                mainContent.Orientation = StackOrientation.Vertical;
        }
    }
}