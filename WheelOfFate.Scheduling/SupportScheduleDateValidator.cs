using System;
using NodaTime;

namespace WheelOfFate.Scheduling
{
    public class SupportScheduleDateValidator : ISupportScheduleDateValidator
    {
        private readonly ICalendar _calendar;

        public SupportScheduleDateValidator(ICalendar calendar)
        {
            _calendar = calendar;
        }

        public void EnsureValid(LocalDate scheduleDate)
        {
            var today = _calendar.Today;

            if (_calendar.IsWeekend(scheduleDate))
            {
                throw new SchedulingException("Cannot schedule support shifts on the weekend");
            }

            if (scheduleDate < today)
            {
                throw new SchedulingException("Cannot schedule support shifts in the past");
            }
        }
    }
}
