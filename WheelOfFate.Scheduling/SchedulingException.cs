using System;

namespace WheelOfFate.Scheduling
{
	public class SchedulingException : Exception
    {
        public SupportScheduleSpecification SupportScheduleSpecification { get; protected set; }
        
        public SchedulingException()
        {}

        public SchedulingException(string message) : base(message)
        {}

        public SchedulingException(string message, Exception inner) : base(message, inner)
        {}

        public SchedulingException(SupportScheduleSpecification supportScheduleSpecification)
        {
            SupportScheduleSpecification = supportScheduleSpecification;
        }

        public SchedulingException(SupportScheduleSpecification supportScheduleSpecification, string message) : base(message)
        {
            SupportScheduleSpecification = supportScheduleSpecification;
        }

        public SchedulingException(SupportScheduleSpecification supportScheduleSpecification, string message, Exception inner) : base(message, inner)
        {
            SupportScheduleSpecification = supportScheduleSpecification;
        }
    }
}
