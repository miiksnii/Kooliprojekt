﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Data;
using Kooliprojekt.Services;
using NuGet.ContentModel;

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
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _projectListService.List(page, 5));
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
        [ValidateAntiForgeryToken]
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

            var projectList = await _projectListService.Get(id.Value);  // Get by id
            if (projectList == null)
            {
                return NotFound();
            }
            return View(projectList);
        }

        // POST: ProjectLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]


        // GET: ProjectLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ProjectLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectList = await _projectListService.Get(id);  // Get by id
            if (projectList != null)
            {
                await _projectListService.Delete(id);  // Use service to delete
            }

            return RedirectToAction(nameof(Index));
        }

        public bool ProjectListExists(int id)
        {
            return _projectListService.Get(id) != null;  // Use service to check existence
        }
    }
}
