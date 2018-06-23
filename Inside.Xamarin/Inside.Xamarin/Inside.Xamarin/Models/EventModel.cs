using System;
using System.Collections.Generic;
using System.Text;
using Inside.Domain.Enum;
using Inside.Xamarin.Models.DomainEnums;

namespace Inside.Xamarin.Models
{
   public class EventModel:BaseModel
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<MyMonthOfYear> MonthRepeat { get; set; }
        public List<MyDayOfWeek> WeekRepeat { get; set; }
    }
}
