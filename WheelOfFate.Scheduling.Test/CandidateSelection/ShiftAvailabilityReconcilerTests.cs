using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using NodaTime;
using WheelOfFate.Scheduling;
using WheelOfFate.Scheduling.CandidateSelection;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;
using Xunit;

namespace WheelOfFate.Scheduling.Test.CandidateSelection
{
    public class ShiftAvailabilityReconcilerTests
    {
        private static readonly LocalDate _scheduleDate = new LocalDate(2018, 5, 25);
        private static readonly Engineer _firstEngineer = new Engineer(1, "1", "1");
        private static readonly Engineer _secondEngineer = new Engineer(2, "2", "2");
        private static readonly Engineer _thirdEngineer = new Engineer(3, "3", "3");
        private static readonly IEnumerable<Engineer> _engineers = new[] { _firstEngineer, _secondEngineer, _thirdEngineer };

        public class SingleShift
        {
            private ShiftAvailabilityReconciler _sut;
            
            public SingleShift()
            {
                var schedules = new[]
                {
                    new DailySchedule(_scheduleDate.PlusDays(-2), new [] { _firstEngineer }),
                    new DailySchedule(_scheduleDate.PlusDays(-1), new [] { _firstEngineer, _secondEngineer })
                };
                var supportScheduleRepository = Mock.Of<ISupportScheduleRepository>(
                    repo => repo.List(It.IsAny<LocalDate>(), It.IsAny<LocalDate>()) == schedules);

                _sut = new ShiftAvailabilityReconciler(supportScheduleRepository);
            }

            [Fact]
            public void GivenNoBiweeklyTracking_Reconcile_ReturnsAllEngineers()
            {
                var result = _sut.Reconcile(_engineers, _scheduleDate, true, false);

                Assert.Equal(_engineers, result);
            }

            [Fact]
            public void GivenBiweeklyTracking_Reconcile_ReturnsOnlyEngineersNotScheduledTwice()
            {
                var result = _sut.Reconcile(_engineers, _scheduleDate, true, true);

                Assert.Equal(new [] { _secondEngineer, _thirdEngineer }, result);
            }
        }

        public class DoubleShift
        {
            private ShiftAvailabilityReconciler _sut;

            public DoubleShift()
            {
                var schedules = new[]
                {
                    new DailySchedule(_scheduleDate.PlusDays(-2), new [] { _firstEngineer }),
                    new DailySchedule(_scheduleDate.PlusDays(-1), new [] { _firstEngineer, _secondEngineer })
                };
                var supportScheduleRepository = Mock.Of<ISupportScheduleRepository>(
                    repo => repo.List(It.IsAny<LocalDate>(), It.IsAny<LocalDate>()) == schedules);

                _sut = new ShiftAvailabilityReconciler(supportScheduleRepository);
            }

            [Fact]
            public void GivenNoBiweeklyTracking_Reconcile_ReturnsAllEngineersTwice()
            {
                var result = _sut.Reconcile(_engineers, _scheduleDate, false, false);

                Assert.Equal(_engineers.Concat(_engineers), result);
            }

            [Fact]
            public void GivenBiweeklyTracking_Reconcile_ReturnsComplimentaryEngineerEntries()
            {
                var result = _sut.Reconcile(_engineers, _scheduleDate, false, true);

                Assert.Equal(0, result.Count(e => e == _firstEngineer));
                Assert.Equal(1, result.Count(e => e == _secondEngineer));
                Assert.Equal(2, result.Count(e => e == _thirdEngineer));
            }
        }
    }
}
