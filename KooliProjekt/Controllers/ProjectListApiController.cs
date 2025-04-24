using KooliProjekt.Data;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using Kooliprojekt.Data;

namespace KooliProjekt.Controllers
{
    [Route("api/ProjectList")]
    [ApiController]
    public class ProjectListApiController : ControllerBase  // Renamed the controller to ProjectListApiController
    {
        private readonly IProjectListService _service;

        public ProjectListApiController(IProjectListService service)  // Renamed constructor
        {
            _service = service;
        }

        // GET: api/ProjectList
        [HttpGet]
        public async Task<IEnumerable<ProjectList>> Get()  // Changed TodoList to ProjectList
        {
            var result = await _service.List(1, 10000);
            return result.Results;
        }

        // GET api/ProjectList/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)  // Changed TodoList to ProjectList
        {
            var list = await _service.Get(id);
            if (list == null)
            {
                return NotFound();
            }

            return list;
        }

        // POST api/ProjectList
        [HttpPost]
        public async Task<object> Post([FromBody] ProjectList list)  // Changed TodoList to ProjectList
        {
            await _service.Save(list);

            return Ok(list);
        }

        // PUT api/ProjectList/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectList list)  // Changed TodoList to ProjectList
        {
            if (id != list.Id)
            {
                return BadRequest();
            }

            await _service.Save(list);

            return Ok();
        }

        // DELETE api/ProjectList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)  // Changed TodoList to ProjectList
        {
            var list = await _service.Get(id);
            if (list == null)
            {
                return NotFound();
            }

            await _service.Delete(id);

            return Ok();
        }
    }
}
