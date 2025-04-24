using Kooliprojekt.Data;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/WorkLogs")]
    [ApiController]
    public class WorkLogApiController : ControllerBase
    {
        private readonly IWorkLogService _workLogService;
        private readonly IProjectItemService _projectItemService;

        public WorkLogApiController(
            IWorkLogService workLogService,
            IProjectItemService projectItemService)
        {
            _workLogService = workLogService;
            _projectItemService = projectItemService;
        }

        // GET: api/WorkLogs
        [HttpGet]
        public async Task<IEnumerable<WorkLog>> Get()
        {
            var result = await _workLogService.List(1, 10000); // Paginate with a large page size (10000) to get all work logs
            return result.Results;
        }

        // GET api/WorkLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkLog>> Get(int id)
        {
            var workLog = await _workLogService.Get(id);
            if (workLog == null)
            {
                return NotFound();
            }

            return Ok(workLog);
        }

        // POST api/WorkLogs
        [HttpPost]
        public async Task<ActionResult<WorkLog>> Post([FromBody] WorkLog workLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _workLogService.Save(workLog);

            // Return CreatedAtAction to ensure the location of the newly created resource is known
            return CreatedAtAction(nameof(Get), new { id = workLog.Id }, workLog);
        }

        // PUT api/WorkLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WorkLog workLog)
        {
            if (id != workLog.Id)
            {
                return BadRequest("ID in URL and body must match.");
            }

            // Check if the work log exists
            var existingWorkLog = await _workLogService.Get(id);
            if (existingWorkLog == null)
            {
                return NotFound();
            }

            // If the work log exists, save the updates
            await _workLogService.Save(workLog);

            return Ok(workLog); // Return the updated work log
        }

        // DELETE api/WorkLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workLog = await _workLogService.Get(id);
            if (workLog == null)
            {
                return NotFound();
            }

            await _workLogService.Delete(id);
            return NoContent(); // No content, but successful deletion
        }
    }
}
