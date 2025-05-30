using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Data;
using Kooliprojekt.Services;
using Kooliprojekt.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Build.Evaluation;

namespace Kooliprojekt.Controllers
{
    public class ProjectListsController : Controller
    {
        private readonly IProjectListService _projectListService;


        public ProjectListsController(IProjectListService projectListService)
        {
            _projectListService = projectListService;
        }

        // GET: ProjectLists
        public async Task<IActionResult> Index(int page = 1, ProjectListIndexModel model = null)
        {
            model = model ?? new ProjectListIndexModel();
            model.Data = await _projectListService.List(page, 5, model.Search);
            return View(model);
        }

        // GET: ProjectLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectList = await _projectListService.Get(id.Value);  // Get by id
            if (projectList == null)
            {
                return NotFound();
            }

            return View(projectList);
        }

        // GET: ProjectLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProjectLists/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] ProjectList projectList)
        {
            if (ModelState.IsValid)
            {
                await _projectListService.Save(projectList);  // Save using the service
                return RedirectToAction(nameof(Index));
            }
            return View(projectList);
        }

        // GET: ProjectLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectList = await _projectListService.Get(id.Value);
            if (projectList == null)
            {
                return NotFound();
            }
            return View(projectList);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectList list)
        {
            if (id != list.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _projectListService.Save(list);
                return RedirectToAction(nameof(Index));
            }

            return View(list);
        }
        // GET: ProjectLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _projectListService.Get(id.Value);
            if (projectItem == null)
            {
                return NotFound();
            }

            return View(projectItem);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectListService.DeleteWithDependenciesAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public bool ProjectListExists(int id)
        {
            // Synchronously wait for the result of Get
            var projectList = _projectListService.Get(id).Result;  // .Result will block until the task completes
            return projectList != null;
        }
    }
}
 