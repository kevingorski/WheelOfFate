using NodaTime;

namespace WheelOfFate.Scheduling
{
    public interface ISupportScheduleDateValidator
    {
        void EnsureValid(LocalDate scheduleDate);
    }
}