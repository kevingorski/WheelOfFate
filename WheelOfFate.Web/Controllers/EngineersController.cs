using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WheelOfFate.Scheduling.Engineers;

namespace WheelOfFate.Web.Controllers
{
    /// <summary>
    /// Engineers controller.
    /// </summary>
    [Route("api/[controller]")]
    public class EngineersController : ControllerBase
    {
        private readonly EngineerRepository _engineerRepository = new EngineerRepository();

        /// <summary>
        /// Gets a list of all engineers.
        /// </summary>
        /// <returns>The list of all engineers.</returns>
        /// <response code="200">Returns all engineers.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Engineer>), (int)HttpStatusCode.OK)]
        public IEnumerable<Engineer> Get()
        {
            return _engineerRepository.List();
        }

        /// <summary>
        /// Gets the engineer with the specified ID (if any)
        /// </summary>
        /// <returns>The engineer.</returns>
        /// <param name="id">Identifier.</param>
        /// <response code="200">Returns the matching engineer.</response>
        /// <response code="404">If no engineer could be found with the given id.</response>
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
