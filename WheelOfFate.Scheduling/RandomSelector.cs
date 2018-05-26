using System;
using System.Collections.Generic;
using System.Linq;

namespace WheelOfFate.Scheduling
{
    public class RandomSelector : IRandomSelector
    {
        private Random _randomGenerator;
        
        public RandomSelector()
        {
            _randomGenerator = new Random(DateTime.Now.GetHashCode());
        }

        /// <summary>
        /// Selects random items from the given <paramref name="source"/>.
        /// </summary>
        /// <returns>The selected items.</returns>
        /// <param name="source">Enumerable source.</param>
        /// <typeparam name="TItem">The type of items in the source enumerable.</typeparam>
        public IEnumerable<TItem> Select<TItem>(IEnumerable<TItem> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var sourceCount = source.Count();
            var selectedItemCount = 0;
            var selectedItemIndicies = new HashSet<int>();

            while(selectedItemCount < sourceCount)
            {
                var index = _randomGenerator.Next(sourceCount);

                if(!selectedItemIndicies.Contains(index))
                {
                    selectedItemIndicies.Add(index);
                    selectedItemCount++;
                    yield return source.ElementAt(index);
                }
            }
        }
    }
}
