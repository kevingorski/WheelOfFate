using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NodaTime;
using Xunit;
using WheelOfFate.Scheduling.CandidateSelection;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Scheduling.Test.CandidateSelection
{
    public class DayOffFilterTests
    {
        private static readonly LocalDate _testDate = new LocalDate(2018, 5, 25);
        private static readonly LocalDate _yesterday = new LocalDate(2018, 05, 24);
        private static readonly Engineer _firstEngineer = new Engineer(1, "1", "1");
        private static readonly Engineer _secondEngineer = new Engineer(2, "2", "2");
        private static readonly Engineer _thirdEngineer = new Engineer(3, "3", "3");
        private static readonly IEnumerable<Engineer> _allEngineers = new[] { _firstEngineer, _secondEngineer, _thirdEngineer };
        private static readonly DailySchedule _yesterdaysSchedule =
            new DailySchedule(_yesterday, _allEngineers.Take(2));


        public class WithNothingScheduledYesterday
        {
            private DayOffFilter _sut;

            public WithNothingScheduledYesterday()
            {
                _sut = new DayOffFilter(Mock.Of<ISupportScheduleRepository>());
            }

            [Fact]
            public void GivenAllEngineers_Filter_ReturnsAllEngineers()
            {
                var result = _sut.Filter(_allEngineers, _testDate);

                Assert.Equal(_allEngineers, result);
            }
        }

        public class WithFirstTwoEngineersScheduledYesterday
        {
            private DayOffFilter _sut;

            public WithFirstTwoEngineersScheduledYesterday()
            {
                var mockSupportScheduleRepository = new Mock<ISupportScheduleRepository>();

                mockSupportScheduleRepository.Setup(scheduleRepository => scheduleRepository.Get(_yesterday))
                                             .Returns(_yesterdaysSchedule);

                _sut = new DayOffFilter(mockSupportScheduleRepository.Object);
            }

            [Fact]
            public void GivenAllEngineers_Filter_ReturnsOnlyThirdEngineer()
            {
                var result = _sut.Filter(_allEngineers, _testDate);

                Assert.Equal(new[] { _thirdEngineer }, result);
            }
        }

        public class WithFirstTwoEngineersScheduledFridayBeforeMonday
        {
            private DayOffFilter _sut;
            private LocalDate _friday = new LocalDate(2018, 5, 18);
            private LocalDate _monday = new LocalDate(2018, 5, 21);

            public WithFirstTwoEngineersScheduledFridayBeforeMonday()
            {
                var mockSupportScheduleRepository = new Mock<ISupportScheduleRepository>();

                mockSupportScheduleRepository.Setup(scheduleRepository => scheduleRepository.Get(_friday))
                                             .Returns(new DailySchedule(_friday, _allEngineers.Take(2)));

                _sut = new DayOffFilter(mockSupportScheduleRepository.Object);
            }

            [Fact]
            public void GivenAllEngineers_Filter_ReturnsOnlyThirdEngineer()
            {
                var result = _sut.Filter(_allEngineers, _monday);

                Assert.Equal(new[] { _thirdEngineer }, result);
            }
        }
    }
}
