using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using WheelOfFate.Scheduling.Engineers;

namespace WheelOfFate.Scheduling.SupportSchedules
{
    public class DailySchedule
    {
        public LocalDate Date { get; }
        public Engineer[] Engineers { get; }

        public DailySchedule(LocalDate date, IEnumerable<Engineer> engineers)
        {
            Date = date;
            Engineers = engineers.ToArray();
        }
    }
}
