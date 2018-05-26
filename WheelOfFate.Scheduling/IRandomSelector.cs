using System.Collections.Generic;

namespace WheelOfFate.Scheduling
{
    public interface IRandomSelector
    {
        IEnumerable<TItem> Select<TItem>(IEnumerable<TItem> source);
    }
}