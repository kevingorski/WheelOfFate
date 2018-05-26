using System;

namespace WheelOfFate.Scheduling.Engineers
{
    public class Engineer : IEquatable<Engineer>
    {
        public int Id
        {
            get;
            private set;
        }
        
        public string FirstName
        {
            get;
            private set;
        }

        public string LastName
        {
            get;
            private set;
        }

        public Engineer(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public bool Equals(Engineer other)
        {
            return other != null && this.Id == other.Id;
        }
    }
}
