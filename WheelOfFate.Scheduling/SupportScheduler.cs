using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using WheelOfFate.Scheduling.CandidateSelection;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Scheduling
{
    public class SupportScheduler : IStartable
    {
        private readonly IRandomSelector _randomSelector;
        private readonly ICalendar _calendar;
        private readonly ISupportScheduleRepository _supportScheduleRepository;
        private readonly ISupportScheduleDateValidator _supportScheduleDateValidator;
        private readonly ISupportCandidateSelector _supportCandidateSelector;

        private const int ShiftsPerDay = 2;

        public SupportScheduler(
            ICalendar calendar,
            ISupportScheduleRepository supportScheduleRepository,
            ISupportScheduleDateValidator supportScheduleDateValidator,
            ISupportCandidateSelector supportCandidateSelector,
            IRandomSelector randomSelector)
        {
            _calendar = calendar;
            _supportScheduleRepository = supportScheduleRepository;
            _supportScheduleDateValidator = supportScheduleDateValidator;
            _supportCandidateSelector = supportCandidateSelector;
            _randomSelector = randomSelector;
        }

        public DailySchedule Schedule(SupportScheduleSpecification supportScheduleSpecification)
        {
            var scheduleDate = supportScheduleSpecification.Date;

            _supportScheduleDateValidator.EnsureValid(scheduleDate);

            return ScheduleWithoutValidation(supportScheduleSpecification);
        }

        private DailySchedule ScheduleWithoutValidation(SupportScheduleSpecification supportScheduleSpecification)
        {
            var supportCandidates = _supportCandidateSelector.Select(supportScheduleSpecification);

            if (supportCandidates.Count() < ShiftsPerDay)
            {
                throw new UnableToMeetSpecificationSchedulingException(
                    supportScheduleSpecification,
                    "Not enough engineers meet requirements to schedule a full day");
            }

            var scheduledEngineers = _randomSelector.Select(supportCandidates).Take(ShiftsPerDay);
            var dailySchedule = new DailySchedule(supportScheduleSpecification.Date, scheduledEngineers);

            _supportScheduleRepository.Save(dailySchedule);

            return dailySchedule;
        }

        /// <summary>
        /// Start this instance - schedule enough days to look interesting
        /// </summary>
        public void Start()
        {
            var today = _calendar.Today;

            for (var date = today.PlusDays(-10); date < today; date = date.PlusDays(1))
            {
                if (!_calendar.IsWeekend(date))
                {
                    try
                    {
                        ScheduleWithoutValidation(new SupportScheduleSpecification(date, true, true, true));
                    }
                    catch
                    {
                        // Swallowing exception to allow for smooth app startup
                        // With all rules enabled it's possible to not be able to schedule the last day
                    }               
                }
            }
        }
    }
}
