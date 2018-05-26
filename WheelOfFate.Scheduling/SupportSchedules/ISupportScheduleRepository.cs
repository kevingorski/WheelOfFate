using System.Collections.Generic;
using NodaTime;

namespace WheelOfFate.Scheduling.SupportSchedules
{
    public interface ISupportScheduleRepository
    {
        DailySchedule Get(LocalDate date);
        IEnumerable<DailySchedule> List(LocalDate from, LocalDate to);
        void Save(DailySchedule dailySchedule);
    }
}