using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using NodaTime;
using WheelOfFate.Scheduling;
using WheelOfFate.Scheduling.CandidateSelection;
using WheelOfFate.Scheduling.Engineers;
using Xunit;

namespace WheelOfFate.Scheduling.Test
{
    public class SupportSchedulerTests
    {
        private readonly SupportScheduleSpecification _specification = 
            new SupportScheduleSpecification(new LocalDate(2018, 5, 25), true, true, true);
        
        [Fact]
        public void GivenNoCandidatesReturned_Schedule_ThrowsUnableToMeetSpecificationSchedulingException()
        {
            using(var autoMock = AutoMock.GetLoose())
            {
                autoMock.Mock<ISupportCandidateSelector>()
                        .Setup(selector => selector.Select(_specification))
                        .Returns(Enumerable.Empty<Engineer>());
                
                var sut = autoMock.Create<SupportScheduler>();

                Assert.Throws<UnableToMeetSpecificationSchedulingException>(() => sut.Schedule(_specification));
            }
        }

        [Fact]
        public void GivenTwoCandidatesReturned_Schedule_ReturnsNewSchedule()
        {
            using (var autoMock = AutoMock.GetLoose())
            {
                var firstEngineer = new Engineer(1, "1", "1");
                var secondEngineer = new Engineer(2, "2", "2");
                var engineers = new[] { firstEngineer, secondEngineer };

                autoMock.Mock<ISupportCandidateSelector>()
                        .Setup(selector => selector.Select(_specification))
                        .Returns(engineers);

                autoMock.Mock<IRandomSelector>()
                        .Setup(selector => selector.Select(It.IsAny<IEnumerable<Engineer>>()))
                        .Returns(engineers);

                var sut = autoMock.Create<SupportScheduler>();
                var result = sut.Schedule(_specification);

                Assert.Equal(_specification.Date, result.Date);
                Assert.Equal(engineers, result.Engineers);
            }
        }
    }
}
