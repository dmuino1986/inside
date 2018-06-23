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
    public class MonthOfYearsResolver:IValueResolver<Event,EventViewModel,List<MyMonthOfYear>>
    {
        public List<MyMonthOfYear> Resolve(Event source, EventViewModel destination, List<MyMonthOfYear> destMember, ResolutionContext context)
        {
            List<MyMonthOfYear> list = new List<MyMonthOfYear>();
            string[] monthRepeat = source.MonthRepeat.Split('-');
            foreach (var numberMonth in monthRepeat)
            {
                MyMonthOfYear month =(MyMonthOfYear) Int16.Parse(numberMonth);
                list.Add(month);
            }
            return list;
        }
    }
}
