using System.Collections.Generic;
using NodaTime;
using WheelOfFate.Scheduling.Engineers;

namespace WheelOfFate.Scheduling.CandidateSelection
{
    public interface IDayOffFilter
    {
        IEnumerable<Engineer> Filter(IEnumerable<Engineer> engineers, LocalDate scheduleDate);
    }
}