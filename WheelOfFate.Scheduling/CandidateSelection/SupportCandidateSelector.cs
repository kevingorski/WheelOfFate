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

            return supportCandidates.ToList();
        }
    }
}
