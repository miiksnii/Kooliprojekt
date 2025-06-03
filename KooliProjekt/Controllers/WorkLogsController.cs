using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Data;
using Kooliprojekt.Models;
using Kooliprojekt.Services;

namespace Kooliprojekt.Controllers
{
    public class WorkLogsController : Controller
    {
        private readonly IWorkLogService _workLogService;
        private readonly IProjectItemService _projectItemService;

        public WorkLogsController(
            IWorkLogService workLogService,
            IProjectItemService projectItemService)
        {
            _workLogService = workLogService;
            _projectItemService = projectItemService;
        }

        // GET: WorkLogs
        public async Task<IActionResult> Index(int page = 1, WorkLogIndexModel model = null)
        {
            model ??= new WorkLogIndexModel();
            model.Data = await _workLogService.List(page, 10, model.Search);
            return View(model);
        }

        // GET: WorkLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workLog = await _workLogService.Get(id.Value);
            if (workLog == null)
            {
                return NotFound();
            }

            return View(workLog);
        }

        // GET: WorkLogs/Create
        public async Task<IActionResult> Create()
        {
            var projectItems = await _projectItemService.List(1, 100);
            ViewBag.ProjectIListId = new SelectList(projectItems.Results, "Id", "Title");
            return View();
        }

        // POST: WorkLogs/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,TimeSpentInMinutes,WorkerName,Description,ProjectIListId")] WorkLog workLog)
        {
            if (ModelState.IsValid)
            {
                await _workLogService.Save(workLog);
                return RedirectToAction(nameof(Index));
            }

            var projectItems = await _projectItemService.List(1, 100);
            ViewBag.ProjectIListId = new SelectList(projectItems.Results, "Id", "Title");
            return View(workLog);
        }

        // GET: WorkLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workLog = await _workLogService.Get(id.Value);
            if (workLog == null)
            {
                return NotFound();
            }

            var projectItems = await _projectItemService.List(1, 100);
            ViewBag.ProjectIListId = new SelectList(projectItems.Results, "Id", "Title", workLog.Id);
            return View(workLog);
        }

        // POST: WorkLogs/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,TimeSpentInMinutes,WorkerName,Description")] WorkLog workLog)
        {
            if (id != workLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _workLogService.Save(workLog);
                return RedirectToAction(nameof(Index));
            }

            var projectItems = await _projectItemService.List(1, 100);
            ViewBag.ProjectIListId = new SelectList(projectItems.Results, "Id", "Title", workLog.Id);
            return View(workLog);
        }

        // GET: WorkLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workLog = await _workLogService.Get(id.Value);
            if (workLog == null)
            {
                return NotFound();
            }

            return View(workLog);
        }

        // POST: WorkLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _workLogService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}