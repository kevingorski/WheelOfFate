using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using NodaTime.Text;
using WheelOfFate.Scheduling;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Web.Controllers
{
    /// <summary>
    /// Support schedules controller.
    /// </summary>
    [Route("api/[controller]")]
    public class SupportSchedulesController : Controller
    {
        private readonly ICalendar _calendar;
        private readonly ISupportScheduleRepository _supportScheduleRepository;
        private readonly SupportScheduler _supportScheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WheelOfFate.Web.Controllers.SupportSchedulesController"/> class.
        /// </summary>
        /// <param name="calendar">Calendar.</param>
        /// <param name="supportScheduleRepository">Support schedule repository.</param>
        /// <param name="supportScheduler">Support scheduler.</param>
        public SupportSchedulesController(
            ICalendar calendar,
            ISupportScheduleRepository supportScheduleRepository,
            SupportScheduler supportScheduler)
        {
            _calendar = calendar;
            _supportScheduleRepository = supportScheduleRepository;
            _supportScheduler = supportScheduler;
        }

        /// <summary>
        /// Get the support schedule for the specified date (if any).
        /// </summary>
        /// <returns>The daily support schedule.</returns>
        /// <param name="date">The date.</param>
        /// <response code="200">Returns the matching schedule.</response>
        /// <response code="400">If no date was supplied or the date supplied could not be parsed as an ISO date</response>
        /// <response code="404">If no schedule could be found with the given date.</response>
        [HttpGet("{date}", Name = "GetSupportSchedule")]
        [ProducesResponseType(typeof(DailySchedule), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Get(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return BadRequest();
            }

            var parseResult = LocalDatePattern.Iso.Parse(date);

            if (!parseResult.Success)
            {
                return BadRequest();
            }

            var dailySchedule = _supportScheduleRepository.Get(parseResult.Value);

            if (dailySchedule == null)
            {
                return NotFound();
            }

            return Ok(dailySchedule);
        }

        /// <summary>
        /// List the support schedules between the specified minDate and maxDate (or last two weeks through next week if not supplied).
        /// </summary>
        /// <returns>The list of support schedules.</returns>
        /// <param name="minDate">Minimum date.</param>
        /// <param name="maxDate">Maximum date.</param>
        /// <response code="200">Returns the matching schedules.</response>
        /// <response code="400">If one or both of the dates supplied could not be parsed as ISO dates</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DailySchedule>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult List([FromQuery] string minDate, [FromQuery] string maxDate)
        {
            var from = _calendar.Today.PlusWeeks(-2);
            var to = _calendar.Today.PlusWeeks(1);

            if(!string.IsNullOrWhiteSpace(minDate))
            {
                var parseResult = LocalDatePattern.Iso.Parse(minDate);

                if(parseResult.Success)
                {
                    from = parseResult.Value;
                }
                else
                {
                    return BadRequest();
                }
            }

            if(!string.IsNullOrWhiteSpace(maxDate))
            {
                var parseResult = LocalDatePattern.Iso.Parse(minDate);

                if(parseResult.Success)
                {
                    to = parseResult.Value;
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok(_supportScheduleRepository.List(from, to));
        }

        /// <summary>
        /// Generate a random support schedule that conforms to the given rules for the given date.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /SupportSchedules
        ///     {
        ///         date: '2018-01-01',
        ///         requireSingleShiftPerEngineerPerDay: true,
        ///         requireDayOffBetweenDaysWithShifts: true,
        ///         requireTwoShiftsInTwoWeeks: true
        ///     }
        /// 
        /// </remarks>
        /// <returns>The generated support schedule.</returns>
        /// <param name="supportScheduleSpecification">The support schedule specification.</param>
        /// <response code="201">Returns the generated schedule.</response>
        /// <response code="400">If the specification was not supplied or the date being scheduled does not pass validation</response>
        /// <response code="409">If a random support schedule could not be generated with the given rules enabled.</response>  
        [HttpPost]
        [ProducesResponseType(typeof(DailySchedule), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public IActionResult Post([FromBody]SupportScheduleSpecification supportScheduleSpecification)
        {
            if (supportScheduleSpecification == null)
            {
                return BadRequest();
            }

            DailySchedule value;

            try
            {
                value = _supportScheduler.Schedule(supportScheduleSpecification);
            }
            catch (UnableToMeetSpecificationSchedulingException ex)
            {
                return StatusCode((int)HttpStatusCode.Conflict, new { ex.Message });
            }
            catch (SchedulingException ex)
            {
                return BadRequest(new { ex.Message });
            }

            return CreatedAtRoute("GetSupportSchedule", new { date = LocalDatePattern.Iso.Format(value.Date) }, value);
        }
    }
}
