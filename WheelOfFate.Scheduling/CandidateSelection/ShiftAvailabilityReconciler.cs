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
            Dictionary<int, IGrouping<int, Engineer>> recentlyScheduledEngineers;

            if(requireSingleShiftPerEngineerPerDay)
            {
                if (requireTwoShiftsInTwoWeeks)
                {
                    // Single shift, two in two weeks = remove candidates already scheduled twice
                    recentlyScheduledEngineers = GetRecentlyScheduledEngineers(scheduleDate);

                    return engineers.Where(engineer =>
                                           !recentlyScheduledEngineers.ContainsKey(engineer.Id)
                                           || recentlyScheduledEngineers[engineer.Id].Count() < 2);
                }
                else
                {
                    // Single shift, not two in two weeks = no change
                    return engineers;
                }
            }

            // Not single shift, not two in two weeks = double all
            if(!requireTwoShiftsInTwoWeeks)
            {
                return engineers.Concat(engineers);
            }

            // Not single shift, two in two weeks = 2 - number of times seen
            recentlyScheduledEngineers = GetRecentlyScheduledEngineers(scheduleDate);

            var atLeastOneShiftAvailableEngineers = engineers.Where(engineer =>
               !recentlyScheduledEngineers.ContainsKey(engineer.Id)
               || recentlyScheduledEngineers[engineer.Id].Count() < 2);
            var twoShiftsAvailableEngineers = engineers.Where(engineer =>
               !recentlyScheduledEngineers.ContainsKey(engineer.Id));
            
            return atLeastOneShiftAvailableEngineers.Concat(twoShiftsAvailableEngineers);
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
