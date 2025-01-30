using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Data;
using Kooliprojekt.Services;

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

            var projectList = await _projectListService.Get(id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] ProjectList projectList)
        {
            if (ModelState.IsValid)
            { 
                await _projectListService.Save(projectList);
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

            var projectList = await _projectListService.Get(id);
            if (projectList == null)
            {
                return NotFound();
            }
            return View(projectList);
        }

        // POST: ProjectLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] ProjectList projectList)
        {
            if (id != projectList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_projectListService.Update(projectList);
                    await _projectListService.Save(projectList);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectListExists(projectList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(projectList);
        }

        // GET: ProjectLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectList = await _projectListService.Get(id);
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
            var projectList = await _projectListService.Get(id);
            if (projectList != null)
            {
                _projectListService.Delete(id);
            }

            await _projectListService.Save(projectList);
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectListExists(int id)
        {
            return _projectListService.Equals(id);
        }
    }
}
