using System;
using System.Linq;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Services;
using Inside.Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inside.Xamarin.Views.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabsPage : TabbedPage
    {
        TabsPageViewModel vm;
        public TabsPage()
        {
            InitializeComponent();

            //TODO: Removed if uneeded! 
            vm = MainViewModel.GetInstance().Tabs;
            BindingContext = vm;

            OnInit();
        }

        private void OnInit()
        {
            MessagesSubcriber();
          
        }

        private void MessagesSubcriber() {

            MessagingCenter.Subscribe<NavigationService>(this, Messages.FocusCoinsTab, (sender) =>
            {
                this.FocusCoinsTab();
            });
        }

        private void FocusCoinsTab() {
            this.CurrentPage = this.coin;
        }



    }
}