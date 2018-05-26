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
        private static readonly IEnumerable<Engineer> _engineers = new[]
        { 
            new Engineer(1, "1", "1"),
            new Engineer(2, "2", "2"),
            new Engineer(3, "3", "3")
        };

        [Fact]
        public void GivenASetOfItems_Select_ReturnsAllOfThem()
        {
            var results = _sut.Select(_engineers);

            Assert.Equal(_engineers, results.OrderBy(e => e.Id));
        }
    }
}
