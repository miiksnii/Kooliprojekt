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
    public class ProjectListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProjectLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectList.ToListAsync());
        }

        // GET: ProjectLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectList = await _context.ProjectList
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(projectList);
                await _context.SaveChangesAsync();
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

            var projectList = await _context.ProjectList.FindAsync(id);
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
                    _context.Update(projectList);
                    await _context.SaveChangesAsync();
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

            var projectList = await _context.ProjectList
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var projectList = await _context.ProjectList.FindAsync(id);
            if (projectList != null)
            {
                _context.ProjectList.Remove(projectList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectListExists(int id)
        {
            return _context.ProjectList.Any(e => e.Id == id);
        }
    }
}
