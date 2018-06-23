using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inside.Domain.Entities;
using Inside.Domain.Enum;
using Inside.WebApi.ViewModels;

namespace Inside.WebApi.MapperTools
{
    public class DayOfWeeksResolver:IValueResolver<Event, EventViewModel, List<MyDayOfWeek>>
    {
        public List<MyDayOfWeek> Resolve(Event source, EventViewModel destination, List<MyDayOfWeek> destMember, ResolutionContext context)
        {
            List<MyDayOfWeek> list = new List<MyDayOfWeek>();
            string[] daysRepeat = source.WeekRepeat.Split('-');
            foreach (var dayNumber in daysRepeat)
            {
                MyDayOfWeek day = (MyDayOfWeek)Int16.Parse(dayNumber);
                list.Add(day);
            }
            return list;
        }
    }
}
