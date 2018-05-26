using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace WheelOfFate.Scheduling
{
    public class SupportScheduleSpecification
    {
        [Required]
        public LocalDate Date { get; }

        [DefaultValue(true)]
        public bool RequireSingleShiftPerEngineerPerDay { get; }

        [DefaultValue(true)]
        public bool RequireDayOffBetweenDaysWithShifts { get; }

        [DefaultValue(true)]
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
