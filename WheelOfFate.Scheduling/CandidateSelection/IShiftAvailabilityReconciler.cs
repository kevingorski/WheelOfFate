using System.Collections.Generic;
using NodaTime;
using WheelOfFate.Scheduling.Engineers;

namespace WheelOfFate.Scheduling.CandidateSelection
{
    public interface IShiftAvailabilityReconciler
    {
        IEnumerable<Engineer> Reconcile(IEnumerable<Engineer> engineers, LocalDate scheduleDate, bool requireSingleShiftPerEngineerPerDay, bool requireTwoShiftsInTwoWeeks);
    }
}