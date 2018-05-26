using NodaTime;

namespace WheelOfFate.Scheduling
{
    public interface ICalendar
    {
        LocalDate Today { get; }

        bool IsWeekend(LocalDate date);
    }
}