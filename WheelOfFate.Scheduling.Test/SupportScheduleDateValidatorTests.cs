using System;
using Moq;
using NodaTime;
using Xunit;

namespace WheelOfFate.Scheduling.Test
{
    public class SupportScheduleDateValidatorTests
    {
        private const int TestYear = 2018;
        private const int TestMonth = 5;
        private const int TestDay = 23;
        private const int WeekendDay = 20;
        private const int WeekdayInPast = 21;
        private const int WeekdayInFuture = 24;
        private readonly LocalDate _today = new LocalDate(TestYear, TestMonth, TestDay);
        private readonly SupportScheduleDateValidator _sut;

        public SupportScheduleDateValidatorTests()
        {
            var calendar = new Mock<ICalendar>();

            calendar.Setup(cal => cal.Today).Returns(_today);

            _sut = new SupportScheduleDateValidator(calendar.Object);
        }

        [Fact]
        public void GivenValidSpecification_EnsureValid_DoesNotThrow()
        {
            _sut.EnsureValid(_today);
        }

        [Theory]
        [InlineData(WeekendDay)]
        [InlineData(WeekdayInPast)]
        public void GivenInvalidDate_EnsureValid_ThrowsSchedulingException(int day)
        {
            Assert.Throws<SchedulingException>(() => _sut.EnsureValid(new LocalDate(TestYear, TestMonth, day)));
        }
    }
}
