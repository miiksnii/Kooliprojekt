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
using Kooliprojekt.Models;

namespace Kooliprojekt.Controllers
{
    public class ProjectItemsController : Controller
    {
        private readonly IProjectListService _projectListService;
        private readonly IProjectItemService _projectItemService;

        public ProjectItemsController(
            IProjectListService projectListService,
            IProjectItemService projectItemService)
        {
            _projectListService = projectListService;
            _projectItemService = projectItemService;
        }

        // GET: ProjectItems
        public async Task<IActionResult> Index(int page = 1, ProjectItemIndexModel model = null)
        {
            model = model ?? new ProjectItemIndexModel();
            model.Data = await _projectItemService.List(page, 5, model.Search);
            return View(model);  
        }

        // GET: ProjectLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _projectItemService.Get(id.Value);
            if (projectItem == null)
            {
                return NotFound();
            }

            return View(projectItem);
        }

        // GET: ProjectItems/Create
        public async Task<IActionResult> Create()
        {
            var projectLists = await _projectListService.List(1, 100);
            ViewBag.ProjectListId = new SelectList(projectLists.Results,"Id", "Title");
            return View();
        }

        // POST: ProjectItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectIList projectItem)
        {
            if (ModelState.IsValid)
            {
                await _projectItemService.Save(projectItem);
                return RedirectToAction(nameof(Index));
            }

            var projectLists = await _projectListService.List(1, 100);
            ViewBag.ProjectListId = new SelectList(projectLists.Results, "Id", "Title");

            return View(projectItem);
        }

        // GET: ProjectLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _projectItemService.Get(id.Value);
            if (projectItem == null)
            {
                return NotFound();
            }

            // Add this - same as in Create action
            var projectLists = await _projectListService.List(1, 100);
            ViewBag.ProjectListId = new SelectList(projectLists.Results, "Id", "Title", projectItem.ProjectListId);

            return View(projectItem);
        }

        // POST: TodoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectIList todoList)
        {
            if (id != todoList.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                await _projectItemService.Save(todoList);
                return RedirectToAction(nameof(Index));
            }
            return View(todoList);
        }

        // GET: ProjectLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _projectItemService.Get(id.Value);
            if (projectItem == null)
            {
                return NotFound();
            }

            return View(projectItem);
        }

        // POST: ProjectLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _projectItemService.Get(id) != null)
            {
                await _projectItemService.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> ProjectItemExists(int id)
        {
            return await _projectItemService.Get(id) != null;
        }
    }
}
