using Kooliprojekt.Data;
using Kooliprojekt.Services;
using KooliProjekt.Data;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    [Route("api/ProjectItems")]
    [ApiController]
    public class ProjectItemApiController : ControllerBase
    {
        private readonly IProjectItemService _service;

        public ProjectItemApiController(IProjectItemService service)
        {
            _service = service;
        }

        // GET: api/ProjectItems
        [HttpGet]
        public async Task<IEnumerable<ProjectIList>> Get()  // Ensure this matches the expected return type
        {
            var result = await _service.List(1, 10000);  // Adjust pagination as necessary
            return result.Results;
        }

        // GET api/ProjectItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST api/ProjectItems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectIList item)  // Make sure you're using the right model here
        {
            await _service.Save(item);
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);  // Make sure the item has an Id property
        }

        // PUT api/ProjectItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectIList item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _service.Save(item);

            return Ok();
        }

        // DELETE api/ProjectItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            await _service.Delete(id);

            return Ok();
        }
    }
}
