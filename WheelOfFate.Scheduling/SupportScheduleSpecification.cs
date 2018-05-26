using System;
using System.Collections.Generic;
using NodaTime;

namespace WheelOfFate.Scheduling
{
    public class SupportScheduleSpecification
    {
        public LocalDate Date { get; }
        public bool RequireSingleShiftPerEngineerPerDay { get; }
        public bool RequireDayOffBetweenDaysWithShifts { get; }
        public bool RequireTwoShiftsInTwoWeeks { get; }

        public SupportScheduleSpecification(
            LocalDate date,
            bool requireSingleShiftPerEngineerPerDay,
            bool requireDayOffBetweenDaysWithShifts,
            bool requireTwoShiftsInTwoWeeks)
        {
            Date = date;
            RequireSingleShiftPerEngineerPerDay = requireSingleShiftPerEngineerPerDay;
            RequireDayOffBetweenDaysWithShifts = requireDayOffBetweenDaysWithShifts;
            RequireTwoShiftsInTwoWeeks = requireTwoShiftsInTwoWeeks;
        }
    }
}
