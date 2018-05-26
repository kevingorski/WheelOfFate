using System;
namespace WheelOfFate.Scheduling
{
	public class UnableToMeetSpecificationSchedulingException : SchedulingException
    {
        public UnableToMeetSpecificationSchedulingException()
        {}

        public UnableToMeetSpecificationSchedulingException(string message) : base(message)
        { }

        public UnableToMeetSpecificationSchedulingException(string message, Exception inner) : base(message, inner)
        { }

        public UnableToMeetSpecificationSchedulingException(SupportScheduleSpecification supportScheduleSpecification)
        {
            SupportScheduleSpecification = supportScheduleSpecification;
        }

        public UnableToMeetSpecificationSchedulingException(SupportScheduleSpecification supportScheduleSpecification, string message) : base(message)
        {
            SupportScheduleSpecification = supportScheduleSpecification;
        }

        public UnableToMeetSpecificationSchedulingException(SupportScheduleSpecification supportScheduleSpecification, string message, Exception inner) : base(message, inner)
        {
            SupportScheduleSpecification = supportScheduleSpecification;
        }
    }
}
