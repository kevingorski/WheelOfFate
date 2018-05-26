using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NodaTime;
using WheelOfFate.Scheduling.CandidateSelection;
using WheelOfFate.Scheduling.Engineers;
using WheelOfFate.Scheduling.SupportSchedules;
using Xunit;
namespace WheelOfFate.Scheduling.Test
{
    public class SupportCandidateSelectorTests
    {
        private static readonly LocalDate _testDate = new LocalDate(2018, 5, 25);
        private static readonly LocalDate _yesterday = new LocalDate(2018, 05, 24);
        private static readonly Engineer _firstEngineer = new Engineer(1, "1", "1");
        private static readonly Engineer _secondEngineer = new Engineer(2, "2", "2");
        private static readonly Engineer _thirdEngineer = new Engineer(3, "3", "3");
        private static readonly Engineer _fourthEngineer = new Engineer(4, "4", "4");
        private static readonly IEnumerable<Engineer> _allEngineers = new[]
        {
            _firstEngineer,
            _secondEngineer,
            _thirdEngineer,
            _fourthEngineer
        };
        private static readonly IEngineerRepository _engineerRepository =
            Mock.Of<IEngineerRepository>(repo => repo.List() == _allEngineers);
        
        //public class EmptySupportSchedule
        //{
        //    private SupportCandidateSelector _sut;
            
        //    public EmptySupportSchedule()
        //    {
        //        var mockSupportScheduleRepository = new Mock<ISupportScheduleRepository>();

        //        mockSupportScheduleRepository.Setup(
        //            scheduleRepository => scheduleRepository.List(It.IsAny<LocalDate>(), It.IsAny<LocalDate>()))
        //                                     .Returns(Enumerable.Empty<DailySchedule>());

        //        _sut = new SupportCandidateSelector(_engineerRepository, mockSupportScheduleRepository.Object);
        //    }

        //    [Fact]
        //    public void GivenAllRulesOn_Select_ReturnsAllEngineers()
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, true, true, true));

        //        Assert.Equal(_allEngineers, result);
        //    }

        //    [Theory]
        //    [InlineData(true)]
        //    [InlineData(false)]
        //    public void GivenNoSingleShiftPerDayRestriction_Select_ReturnsAllEngineersTwice(bool requireTwoShiftsInTwoWeeks)
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, false, true, requireTwoShiftsInTwoWeeks));

        //        Assert.Equal(_allEngineers.Concat(_allEngineers), result);
        //    }
        //}

        //public class TwoDaySupportSchedule
        //{
        //    private SupportCandidateSelector _sut;
        //    private static readonly IEnumerable<DailySchedule> _dailySchedules = new[]
        //    {
        //        new DailySchedule(new LocalDate(2018, 05, 23), new Engineer[] { _firstEngineer, _secondEngineer }),
        //        new DailySchedule(_yesterday, new Engineer[] { _thirdEngineer, _fourthEngineer })
        //    };

        //    public TwoDaySupportSchedule()
        //    {
        //        var mockSupportScheduleRepository = new Mock<ISupportScheduleRepository>();

        //        mockSupportScheduleRepository.Setup(
        //            scheduleRepository => scheduleRepository.List(It.IsAny<LocalDate>(), It.IsAny<LocalDate>()))
        //                                     .Returns(_dailySchedules);

        //        mockSupportScheduleRepository.Setup(scheduleRepository => scheduleRepository.Get(_yesterday))
        //                                     .Returns(_dailySchedules.Last());

        //        _sut = new SupportCandidateSelector(_engineerRepository, mockSupportScheduleRepository.Object);
        //    }

        //    [Theory]
        //    [InlineData(true)]
        //    [InlineData(false)]
        //    public void Given_Select_ReturnsFirstTwoEngineers(bool requireTwoShiftsInTwoWeeks)
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, true, true, requireTwoShiftsInTwoWeeks));

        //        Assert.Equal(new[] { _firstEngineer, _secondEngineer }, result);
        //    }

        //    [Fact]
        //    public void GivenNoDayOffRequirement_Select_ReturnsAllEngineers()
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, true, false, true));

        //        Assert.Equal(_allEngineers, result);
        //    }
        //}

        //public class FourDaySupportSchedule
        //{
        //    private SupportCandidateSelector _sut;
        //    private static readonly IEnumerable<DailySchedule> _dailySchedules = new[]
        //    {
        //        new DailySchedule(new LocalDate(2018, 05, 21), new Engineer[] { _firstEngineer, _secondEngineer }),
        //        new DailySchedule(new LocalDate(2018, 05, 22), new Engineer[] { _thirdEngineer, _fourthEngineer }),
        //        new DailySchedule(new LocalDate(2018, 05, 23), new Engineer[] { _firstEngineer, _secondEngineer }),
        //        new DailySchedule(_yesterday, new Engineer[] { _thirdEngineer, _fourthEngineer })
        //    };

        //    public FourDaySupportSchedule()
        //    {
        //        var mockSupportScheduleRepository = new Mock<ISupportScheduleRepository>();

        //        mockSupportScheduleRepository.Setup(
        //            scheduleRepository => scheduleRepository.List(It.IsAny<LocalDate>(), It.IsAny<LocalDate>()))
        //                                     .Returns(_dailySchedules);

        //        mockSupportScheduleRepository.Setup(scheduleRepository => scheduleRepository.Get(_yesterday))
        //                                     .Returns(_dailySchedules.Last());

        //        _sut = new SupportCandidateSelector(_engineerRepository, mockSupportScheduleRepository.Object);
        //    }

        //    [Theory]
        //    [InlineData(true, true)]
        //    [InlineData(true, false)]
        //    [InlineData(false, false)]
        //    public void GivenTwoShiftLimit_Select_ReturnsNoEngineers(bool singleShiftPerDay, bool dayOff)
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, singleShiftPerDay, dayOff, true));

        //        Assert.Empty(result);
        //    }

        //    [Fact]
        //    public void GivenOnlySingleShiftRule_Select_ReturnsAllEngineers()
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, true, false, false));

        //        Assert.Equal(_allEngineers, result);
        //    }

        //    [Fact]
        //    public void GivenAllRulesOff_Select_ReturnsAllEngineersTwice()
        //    {
        //        var result = _sut.Select(new SupportScheduleSpecification(_testDate, false, false, false));

        //        Assert.Equal(_allEngineers.Concat(_allEngineers), result);
        //    }
        //}
    }
}
