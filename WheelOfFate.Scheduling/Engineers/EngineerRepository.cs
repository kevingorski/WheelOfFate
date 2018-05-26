using System;
using System.Collections.Generic;
using System.Linq;

namespace WheelOfFate.Scheduling.Engineers
{
    public class EngineerRepository : IEngineerRepository
    {      
        private static readonly Engineer[] _engineers = {
            new Engineer(1, "Alice", "Abba"),
            new Engineer(2, "Bob", "Bannon"),
            new Engineer(3, "Carol", "Cate"),
            new Engineer(4, "Daniel", "Davis"),
            new Engineer(5, "Eunice", "Eldorado"),
            new Engineer(6, "Frederick", "Fakename"),
            new Engineer(7, "Genevive", "Gee"),
            new Engineer(8, "Harold", "Harris"),
            new Engineer(9, "Irene", "Iris"),
            new Engineer(10, "Jordan", "James")
        };

        public IEnumerable<Engineer> List()
        {
            return _engineers;
        }

        public Engineer Get(int id)
        {
            return _engineers.SingleOrDefault(engineerCandidate => engineerCandidate.Id == id);
        }
    }
}
