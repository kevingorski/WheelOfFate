using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Scheduling.CandidateSelection
{
    public class ShiftAvailabilityReconciler : IShiftAvailabilityReconciler
    {
        private readonly ISupportScheduleRepository _supportScheduleRepository;

        public ShiftAvailabilityReconciler(ISupportScheduleRepository supportScheduleRepository)
        {
            _supportScheduleRepository = supportScheduleRepository;
        }

        public IEnumerable<Engineer> Reconcile(
            IEnumerable<Engineer> engineers, 
            LocalDate scheduleDate, 
            bool requireSingleShiftPerEngineerPerDay,
            bool requireTwoShiftsInTwoWeeks)
        {
            if (requireTwoShiftsInTwoWeeks)
            {
                var recentlyScheduledEngineers = GetRecentlyScheduledEngineers(scheduleDate);
                var engineersNotRecentlyScheduledTwice = engineers.Where(engineer =>
                       !recentlyScheduledEngineers.ContainsKey(engineer.Id)
                       || recentlyScheduledEngineers[engineer.Id].Count() < 2);

                if (requireSingleShiftPerEngineerPerDay)
                {
                    // Single shift, two in two weeks = leave only candidates not already scheduled twice
                    return engineersNotRecentlyScheduledTwice;
                }

                // Not single shift, two in two weeks = 2 - number of times seen
                var engineersNotRecentlyScheduledAtAll = engineers.Where(engineer =>
                   !recentlyScheduledEngineers.ContainsKey(engineer.Id));

                return engineersNotRecentlyScheduledTwice.Concat(engineersNotRecentlyScheduledAtAll);
            }

            if(requireSingleShiftPerEngineerPerDay)
            {
                // Single shift, not two in two weeks = no change
                return engineers;
            }

            // Not single shift, not two in two weeks = double all
            return engineers.Concat(engineers);
        }

        private Dictionary<int, IGrouping<int, Engineer>> GetRecentlyScheduledEngineers(LocalDate scheduleDate)
        {
            return _supportScheduleRepository.List(scheduleDate.PlusWeeks(-2), scheduleDate.PlusDays(-1))
                .SelectMany(schedule => schedule.Engineers)
                .GroupBy(engineer => engineer.Id)
                .ToDictionary(group => group.Key);
        }
    }
}
