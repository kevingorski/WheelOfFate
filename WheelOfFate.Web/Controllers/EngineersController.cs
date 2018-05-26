using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WheelOfFate.Scheduling.Engineers;

namespace WheelOfFate.Web.Controllers
{
    [Route("api/[controller]")]
    public class EngineersController : ControllerBase
    {
        private readonly EngineerRepository _engineerRepository = new EngineerRepository();

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Engineer>), (int)HttpStatusCode.OK)]
        public IEnumerable<Engineer> Get()
        {
            return _engineerRepository.List();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Engineer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Get(int id)
        {
            var engineer = _engineerRepository.Get(id);

            if(engineer == null)
            {
                return NotFound();
            }

            return Ok(engineer);
        }
    }
}
