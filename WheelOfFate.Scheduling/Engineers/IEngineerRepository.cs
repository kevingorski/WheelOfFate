using System.Collections.Generic;

namespace WheelOfFate.Scheduling.Engineers
{
    public interface IEngineerRepository
    {
        Engineer Get(int id);
        IEnumerable<Engineer> List();
    }
}