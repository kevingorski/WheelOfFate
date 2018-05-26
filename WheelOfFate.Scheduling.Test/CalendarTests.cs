using System;
using Xunit;
using NodaTime;
using WheelOfFate.Scheduling;

namespace WheelOfFate.Scheduling.Test
{
    public class CalendarTests
    {
        private readonly Calendar _sut = new Calendar();

        [Fact]
        public void GivenWeekday_IsWeekend_ReturnsFalse()
        {
            Assert.False(_sut.IsWeekend(new LocalDate(2018, 05, 21)));
        }

        [Fact]
        public void GivenWeekend_IsWeekend_ReturnsTrue()
        {
            Assert.True(_sut.IsWeekend(new LocalDate(2018, 05, 20)));
        }
    }
}
