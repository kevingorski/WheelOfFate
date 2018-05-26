using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Scheduling.CandidateSelection
{
    public class DayOffFilter : IDayOffFilter
    {
        private readonly ISupportScheduleRepository _supportScheduleRepository;

        public DayOffFilter(ISupportScheduleRepository supportScheduleRepository)
        {
            this._supportScheduleRepository = supportScheduleRepository;
        }

        public IEnumerable<Engineer> Filter(IEnumerable<Engineer> engineers, LocalDate scheduleDate)
        {
            var previousWorkdayOffset = scheduleDate.DayOfWeek == IsoDayOfWeek.Monday ? -3 : -1;
            var previousWorkdaysSchedule = _supportScheduleRepository.Get(scheduleDate.PlusDays(previousWorkdayOffset));

            if (previousWorkdaysSchedule != null)
            {
                var excludedEngineers = previousWorkdaysSchedule.Engineers;

                return engineers.Where(candidate => !excludedEngineers.Contains(candidate));
            }

            return engineers;
        }
    }
}
