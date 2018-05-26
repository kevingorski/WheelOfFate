using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using NodaTime.Text;
using WheelOfFate.Scheduling;
using WheelOfFate.Scheduling.SupportSchedules;

namespace WheelOfFate.Web.Controllers
{
    [Route("api/[controller]")]
    public class SupportSchedulesController : Controller
    {
        private readonly ICalendar _calendar;
        private readonly ISupportScheduleRepository _supportScheduleRepository;
        private readonly SupportScheduler _supportScheduler;

        public SupportSchedulesController(
            ICalendar calendar,
            ISupportScheduleRepository supportScheduleRepository,
            SupportScheduler supportScheduler)
        {
            _calendar = calendar;
            _supportScheduleRepository = supportScheduleRepository;
            _supportScheduler = supportScheduler;
        }

        [HttpGet("{date}")]
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

            return Created(string.Format("http://{0}/api/supportschedules/{1}", Request.Host, LocalDatePattern.Iso.Format(value.Date)), value);
        }
    }
}
