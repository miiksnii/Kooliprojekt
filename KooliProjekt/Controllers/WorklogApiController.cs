using Kooliprojekt.Data;
using Kooliprojekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.PublicApi.Api;
using ApiWorkLog = KooliProjekt.PublicApi.Api.ApiWorkLog;
using DomainWorkLog = Kooliprojekt.Data.WorkLog;


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
        public async Task<ActionResult<Result<List<ApiWorkLog>>>> Get()
        {
            var page = await _workLogService.List(1, 10000);
            var dtos = page.Results
                .Select(d => new ApiWorkLog
                {
                    Id = d.Id,
                    Date = d.Date,
                    TimeSpentInMinutes = d.TimeSpentInMinutes,
                    WorkerName = d.WorkerName,
                    Description = d.Description
                })
                .ToList();
            return Ok(new Result<List<ApiWorkLog>> { Value = dtos });
        }


        // GET api/WorkLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ApiWorkLog>>> Get(int id)
        {
            var d = await _workLogService.Get(id);
            if (d == null)
                return NotFound(new Result<ApiWorkLog> { Error = $"ID {id} not found." });

            var dto = new ApiWorkLog
            {
                Id = d.Id,
                Date = d.Date,
                TimeSpentInMinutes = d.TimeSpentInMinutes,
                WorkerName = d.WorkerName,
                Description = d.Description
            };
            return Ok(new Result<ApiWorkLog> { Value = dto });
        }

        // POST api/WorkLogs
        [HttpPost]
        public async Task<ActionResult<Result<ApiWorkLog>>> Post([FromBody] ApiWorkLog dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                  .Where(x => x.Value.Errors.Any())
                  .ToDictionary(
                    x => x.Key.Split('.').Last(),
                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                  );
                return BadRequest(new Result<ApiWorkLog> { Errors = errors });
            }

            var domain = new DomainWorkLog
            {
                Date = dto.Date,
                TimeSpentInMinutes = dto.TimeSpentInMinutes,
                WorkerName = dto.WorkerName,
                Description = dto.Description
            };
            await _workLogService.Save(domain);
            dto.Id = domain.Id;
            return Ok(new Result<ApiWorkLog> { Value = dto });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Result<ApiWorkLog>>> Put(int id, [FromBody] ApiWorkLog dto)
        {
            if (id != dto.Id)
                return BadRequest(new Result<ApiWorkLog> { Error = "ID mismatch." });

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                  .Where(x => x.Value.Errors.Any())
                  .ToDictionary(
                    x => x.Key.Split('.').Last(),
                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                  );
                return BadRequest(new Result<ApiWorkLog> { Errors = errors });
            }

            var exists = await _workLogService.Get(id);
            if (exists == null)
                return NotFound(new Result<ApiWorkLog> { Error = $"ID {id} not found." });

            var domain = new DomainWorkLog
            {
                Id = dto.Id,
                Date = dto.Date,
                TimeSpentInMinutes = dto.TimeSpentInMinutes,
                WorkerName = dto.WorkerName,
                Description = dto.Description
            };
            await _workLogService.Save(domain);
            return Ok(new Result<ApiWorkLog> { Value = dto });
        }

        // DELETE api/WorkLogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var exists = await _workLogService.Get(id);
            if (exists == null)
                return NotFound(new Result { Error = $"ID {id} not found." });

            await _workLogService.Delete(id);
            return Ok(new Result());
        }
    }
}
