using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Inside.Domain.Enum;
using Inside.Xamarin.Helpers;
using Inside.Xamarin.Models;
using Inside.Xamarin.Models.DomainEnums;
using Inside.Xamarin.Services;
using Xamarin.Forms;

namespace Inside.Xamarin.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;
        private TimeSpan _endTime;
        private ObservableCollection<MyMonthOfYear> _monthRepeat;
        private TimeSpan _startTime;
        private ObservableCollection<DayOfWeek> _weekRepeat;

        public EventViewModel()
        {
            _navigationService = NavigationService.GetInstance();
            ParkingEvent = new EventModel();
        }

        public EventViewModel(EventModel parkingEvent)
        {
            _navigationService = NavigationService.GetInstance();
            ParkingEvent = parkingEvent;
        }

        public ObservableCollection<DayOfWeek> WeekRepeat
        {
            get => _weekRepeat;
            set => SetValue(ref _weekRepeat, value);
        }

        public ObservableCollection<MyMonthOfYear> MonthRepeat
        {
            get => _monthRepeat;
            set => SetValue(ref _monthRepeat, value);
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set => SetValue(ref _startTime, value);
        }

        public EventModel ParkingEvent { get; set; }

        public TimeSpan EndTime
        {
            get => _endTime;
            set => SetValue(ref _endTime, value);
        }

        public ICommand EventCreateCommand => new RelayCommand(EventCreate);

        private async void EventCreate()
        {
           // ParkingEvent = new EventModel();
            //ParkingEvent.StartTime = StartTime;
            //ParkingEvent.EndTime = EndTime;
            //var parkingEvent = new Event
            //{
            //    StartTime = StartTime,
            //    EndTime = EndTime,
            //    MonthRepeat = "1-2-3-4",
            //    WeekRepeat = "1-2-3"
            //};
            //ParkingEvent.MonthRepeat = "1-2-3-4";
            //ParkingEvent.WeekRepeat = "1-2-3";
            ParkingEvent.MonthRepeat = new List<MyMonthOfYear>
            {
                MyMonthOfYear.January,
                MyMonthOfYear.February,
                MyMonthOfYear.March,
                MyMonthOfYear.April
            };
            ParkingEvent.WeekRepeat = new List<MyDayOfWeek>
            {
                MyDayOfWeek.Monday,
                MyDayOfWeek.Tuesday,
                MyDayOfWeek.Wednesday
            };

            MessagingCenter.Send(ParkingEvent, Messages.ParkingEventCreated);
            await _navigationService.BackOnMaster();
        }
    }
}