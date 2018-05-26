using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace WheelOfFate.Scheduling
{
    public class Calendar : ICalendar
    {
        private readonly HashSet<IsoDayOfWeek> _weekendDaysOfWeek = new HashSet<IsoDayOfWeek>
        { 
            IsoDayOfWeek.Saturday, 
            IsoDayOfWeek.Sunday
        };

        public LocalDate Today
        {
            get { return LocalDate.FromDateTime(DateTime.Today); }
        }

        public bool IsWeekend(LocalDate date)
        {
            return _weekendDaysOfWeek.Contains(date.DayOfWeek);
        }
    }
}
