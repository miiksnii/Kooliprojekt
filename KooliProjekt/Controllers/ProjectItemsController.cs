using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kooliprojekt.Data;

namespace Kooliprojekt.Controllers
{
    public class ProjectItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectItems
        public async Task<IActionResult> Index(int page = 1)
        {
            var applicationDbContext = _context.ProjectItem.Include(p => p.ProjectList);
            return View(await applicationDbContext.GetPagedAsync(page, 5));
        }

        // GET: ProjectItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _context.ProjectItem
                .Include(p => p.ProjectList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectItem == null)
            {
                return NotFound();
            }

            return View(projectItem);
        }

        // GET: ProjectItems/Create
        public IActionResult Create()
        {
            ViewData["ProjectListId"] = new SelectList(_context.ProjectList, "Id", "Id");
            return View();
        }

        // POST: ProjectItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Title,StartDate,EstimatedWorkTime,AdminName,Description,IsDone,ProjectListId")] ProjectItem projectItem)
        {
            ModelState.Remove("ProjectList");
            if (ModelState.IsValid)
            {
                _context.Add(projectItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectListId"] = new SelectList(_context.ProjectList, "Id", "Id", projectItem.ProjectListId);
            return View(projectItem);
        }

        // GET: ProjectItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _context.ProjectItem.FindAsync(id);
            if (projectItem == null)
            {
                return NotFound();
            }
            ViewData["ProjectListId"] = new SelectList(_context.ProjectList, "Id", "Id", projectItem.ProjectListId);
            return View(projectItem);
        }

        // POST: ProjectItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Title,StartDate,EstimatedWorkTime,AdminName,Description,IsDone,ProjectListId")] ProjectItem projectItem)
        {
            if (id != projectItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectItemExists(projectItem.Id))
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
            ViewData["ProjectListId"] = new SelectList(_context.ProjectList, "Id", "Id", projectItem.ProjectListId);
            return View(projectItem);
        }

        // GET: ProjectItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectItem = await _context.ProjectItem
                .Include(p => p.ProjectList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectItem == null)
            {
                return NotFound();
            }

            return View(projectItem);
        }

        // POST: ProjectItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectItem = await _context.ProjectItem.FindAsync(id);
            if (projectItem != null)
            {
                _context.ProjectItem.Remove(projectItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectItemExists(int id)
        {
            return _context.ProjectItem.Any(e => e.Id == id);
        }
    }
}
