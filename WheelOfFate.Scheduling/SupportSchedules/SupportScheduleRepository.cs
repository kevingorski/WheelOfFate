using System.Collections.Generic;
using System.Linq;
using Autofac;
using NodaTime;

namespace WheelOfFate.Scheduling.SupportSchedules
{
    public class SupportScheduleRepository : ISupportScheduleRepository
    {
		private static Dictionary<LocalDate, DailySchedule> _dailySchedules = new Dictionary<LocalDate, DailySchedule>();

        public DailySchedule Get(LocalDate date)
        {
            return _dailySchedules.GetValueOrDefault(date);
        }

        public IEnumerable<DailySchedule> List(LocalDate from, LocalDate to)
        {
            return _dailySchedules.Values
                .Where(value => from <= value.Date && value.Date <= to)
                .OrderBy(value => value.Date);
        }

        public void Save(DailySchedule dailySchedule)
        {
            _dailySchedules[dailySchedule.Date] = dailySchedule;
        }
    }
}
