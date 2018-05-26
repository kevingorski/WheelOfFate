using System;
using System.Collections.Generic;
using System.Linq;
using WheelOfFate.Scheduling.Engineers;
using Xunit;

namespace WheelOfFate.Scheduling.Test
{
    public class RandomSelectorTests
    {
        private static readonly RandomSelector _sut = new RandomSelector();
        private static readonly IEnumerable<Engineer> _engineers = new[] { new Engineer(1, "1", "1"), new Engineer(2, "2", "2") };

        [Fact]
        public void GivenEqualNumberOfItemsToSelect_Select_ReturnsAllOfThem()
        {
            var results = _sut.Select(_engineers).Take(2);

            Assert.Equal(2, results.Count());
        }

        [Fact]
        public void GivenSubsetOfItemsToSelect_Select_ReturnsSubset()
        {
            var results = _sut.Select(_engineers).Take(1);

            Assert.Equal(1, results.Count());
        }
    }
}
