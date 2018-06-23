using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Inside.Domain.Core;
using Inside.Domain.Enum;

namespace Inside.Domain.Entities
{
    public class Event:BaseEntity
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string MonthRepeat { get; set; }
        public string WeekRepeat { get; set; }
    }
}
