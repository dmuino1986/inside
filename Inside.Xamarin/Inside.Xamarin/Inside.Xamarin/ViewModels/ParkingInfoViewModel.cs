using GalaSoft.MvvmLight.Command;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using System;
using System.Windows.Input;
using Inside.Xamarin.Models.DomainModels;
using Xamarin.Forms;

namespace Inside.Xamarin.ViewModels
{
    public class ParkingInfoViewModel:BaseViewModel
    {
        public ParkingModel Parking { get; set; }
        public DateTime RentDate { get; set; }
        public TimeSpan RentFrom { get; set; }
        public TimeSpan RentTo { get; set; }
        public ICommand RentCommand => new RelayCommand(Rent);

        public ParkingInfoViewModel(ParkingModel parking)
        {
            this.Parking = parking;
            this.RentDate = DateTime.Now;
            this.RentFrom = DateTime.Now.TimeOfDay;
            this.RentTo = DateTime.Now.TimeOfDay;
        }

        public void Parse()
        {
            
        }

        public async void Rent()
        {
            Order order = new Order
            {
                Date = this.RentDate,
                StartTime = this.RentFrom,
                EndTime = this.RentTo,
                UserId = ((User) Application.Current.Properties["userLoged"]).Id,
                ParkingId = this.Parking.Id
            };
            var response =
                await this.InsideApi.Post<Order, Order>(HostSetting.OrderEndPoint + "/add", order);
            if (response.IsSuccess)
            {
                this.Parking.RentInfo = (Order)response.Result;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                   Languages.GeneralError,
                   Languages.ParkingInfoRentAlert,
                   Languages.GeneralAccept);
            }
        }
    }
}
