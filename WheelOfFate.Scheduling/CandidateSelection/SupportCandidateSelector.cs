using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Scheduling.CandidateSelection
{
    public class SupportCandidateSelector : ISupportCandidateSelector
    {
        private readonly IEngineerRepository _engineerRepository;
        private readonly ISupportScheduleRepository _supportScheduleRepository;
        private readonly IDayOffFilter _dayOffFilter;
        private readonly IShiftAvailabilityReconciler _shiftAvailabilityReconciler;

        public SupportCandidateSelector(
            IEngineerRepository engineerRepository,
            ISupportScheduleRepository supportScheduleRepository,
            IDayOffFilter dayOffFilter,
            IShiftAvailabilityReconciler shiftAvailabilityReconciler)
        {
            _engineerRepository = engineerRepository;
            _supportScheduleRepository = supportScheduleRepository;
            _dayOffFilter = dayOffFilter;
            _shiftAvailabilityReconciler = shiftAvailabilityReconciler;
        }

        public IEnumerable<Engineer> Select(SupportScheduleSpecification supportScheduleSpecification)
        {
            var scheduleDate = supportScheduleSpecification.Date;
            var supportCandidates = _engineerRepository.List();

            if (supportScheduleSpecification.RequireDayOffBetweenDaysWithShifts)
            {
                supportCandidates = _dayOffFilter.Filter(supportCandidates, scheduleDate);
            }

            supportCandidates = _shiftAvailabilityReconciler.Reconcile(
                supportCandidates,
                scheduleDate,
                supportScheduleSpecification.RequireSingleShiftPerEngineerPerDay,
                supportScheduleSpecification.RequireTwoShiftsInTwoWeeks);
            
            //if (supportScheduleSpecification.RequireTwoShiftsInTwoWeeks)
            //{
            //    var recentlyScheduledEngineers = _supportScheduleRepository.List(scheduleDate.PlusDays(-13), scheduleDate.PlusDays(-1))
            //       .SelectMany(schedule => schedule.Engineers)
            //       .GroupBy(engineer => engineer.Id)
            //       .ToDictionary(group => group.Key);

            //    // Remove candidates already scheduled twice
            //    supportCandidates.RemoveAll(candidate =>
            //        recentlyScheduledEngineers.ContainsKey(candidate.Id)
            //        && recentlyScheduledEngineers[candidate.Id].Count() > 1);

            //    if (!supportScheduleSpecification.RequireSingleShiftPerEngineerPerDay)
            //    {
            //        // Add additional entry for candidates that haven't been scheduled recently
            //        supportCandidates = supportCandidates.Concat(
            //            supportCandidates.Where(candidate =>
            //                !recentlyScheduledEngineers.ContainsKey(candidate.Id))).ToList();
            //    }
            //}
            //else
            //{
            //    // No two shifts in two weeks requirement
            //    if (!supportScheduleSpecification.RequireSingleShiftPerEngineerPerDay)
            //    {
            //        // Double up
            //        supportCandidates = supportCandidates.Concat(supportCandidates).ToList();
            //    }
            //}

            return supportCandidates.ToList();
        }
    }
}
