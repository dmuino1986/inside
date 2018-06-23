using System.Collections.Generic;
using Inside.Xamarin.CustomControls;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Services;
using Inside.Xamarin.ViewModels;
using Inside.Xamarin.Views.PopUp;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Inside.Xamarin.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly GeolocatorService _geolocatorService;
        private readonly ApiService _insideApi;


        public HomePage()
        {
            _insideApi = new ApiService(); //TODO: Use DI
            _geolocatorService = new GeolocatorService(); //TODO: Use DI
            InitializeComponent();
            OnInitAsync();

            // This is a callback to get a parking who was created in create parkingviewmodel and show it in the map.
            MessagingCenter.Subscribe<ParkingModel>(this, Messages.NewParkingCreated, newParking =>
            {
                var pos = new Position(double.Parse(newParking.Latitude), double.Parse(newParking.Longitude));
                CreatePin(pos, newParking);
            });
        }

        public List<ParkingModel> Parkings { get; set; }

        private async void OnInitAsync()
        {
            MyMap.Tapped += MyMap_Tapped;
            MyMap.PinTapped += MyMap_PinTapped;

            await _geolocatorService.GetLocation();

            if (_geolocatorService.Lat > 0 || _geolocatorService.Lon > 0)
            {
                var position = new Position(_geolocatorService.Lat, _geolocatorService.Lon);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(.5)));
            }
            else
            {
                await DisplayAlert("Geolocation", "Geolocation service not available!", "Ok");
            }

            GetParkings();
        }

        private async void MyMap_PinTapped(object sender, PinTapEventArgs e)
        {
            var parking = e.Parking;
            if (parking.UserId == MainViewModel.GetInstance().CurrentUser.Id)
                {
                    MainViewModel.GetInstance().ParkingEdit = new ParkingEditViewModel(parking);
                    await NavigationService.GetInstance().NavigateOnMaster(Pages.ParkingEdit);
                }
            else
            {
                MainViewModel.GetInstance().ParkingInfo = new ParkingInfoViewModel(parking);
                await NavigationService.GetInstance().NavigateOnMaster(Pages.ParkingInfo);
            }
           
        }

        private void MyMap_Tapped(object sender, MapTapEventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new CreateParkingPopUp(e.Position));
        }

        private async void GetParkings()
        {
            var response = await _insideApi.GetAllParkings(HostSetting.ParkingEndPoint);

            if (response.Result == null) return;

            Parkings = (List<ParkingModel>) response.Result;

            if (Parkings.Count > 0)
                foreach (var parking in Parkings)
                {
                    var position = new Position(double.Parse(parking.Latitude), double.Parse(parking.Longitude));
                    CreatePin(position, parking);
                }
        }

        private void CreatePin(Position position, ParkingModel parking)
        {
            var pin = new CustomPin
            {
                Id = parking.Id,
                Parking = parking,
                Type = PinType.Place,
                Position = position,
                Label = "custom pin",
                Address = "custom detail info"
            };

            MyMap.Pins.Add(pin);
        }
    }
}