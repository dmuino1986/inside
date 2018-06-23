using System;

namespace Inside.Xamarin.Models.DomainModels
{
    public class Event: BaseModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string MonthRepeat { get; set; }
        public string WeekRepeat { get; set; }
        //public Parking Parking { get; set; }
        //public int ParkingId { get; set; }
    }
}
