using System.Collections.Generic;
using WheelOfFate.Scheduling.Engineers;

namespace WheelOfFate.Scheduling.CandidateSelection
{
    public interface ISupportCandidateSelector
    {
        IEnumerable<Engineer> Select(SupportScheduleSpecification supportScheduleSpecification);
    }
}