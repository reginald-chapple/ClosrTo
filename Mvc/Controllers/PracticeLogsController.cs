using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc.Data;
using Mvc.Entities;

namespace Mvc.Controllers
{
    public class PracticeLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PracticeLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PracticeLogs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PracticeLogs.Include(p => p.Practice).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PracticeLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceLog = await _context.PracticeLogs
                .Include(p => p.Practice)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practiceLog == null)
            {
                return NotFound();
            }

            return View(practiceLog);
        }

        // GET: PracticeLogs/Create
        public IActionResult Create()
        {
            ViewData["PracticeId"] = new SelectList(_context.Practices, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: PracticeLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Reflections,Rating,UserId,PracticeId")] PracticeLog practiceLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(practiceLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PracticeId"] = new SelectList(_context.Practices, "Id", "Id", practiceLog.PracticeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", practiceLog.UserId);
            return View(practiceLog);
        }

        // GET: PracticeLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceLog = await _context.PracticeLogs.FindAsync(id);
            if (practiceLog == null)
            {
                return NotFound();
            }
            ViewData["PracticeId"] = new SelectList(_context.Practices, "Id", "Id", practiceLog.PracticeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", practiceLog.UserId);
            return View(practiceLog);
        }

        // POST: PracticeLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Reflections,Rating,UserId,PracticeId")] PracticeLog practiceLog)
        {
            if (id != practiceLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(practiceLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticeLogExists(practiceLog.Id))
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
            ViewData["PracticeId"] = new SelectList(_context.Practices, "Id", "Id", practiceLog.PracticeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", practiceLog.UserId);
            return View(practiceLog);
        }

        // GET: PracticeLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practiceLog = await _context.PracticeLogs
                .Include(p => p.Practice)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practiceLog == null)
            {
                return NotFound();
            }

            return View(practiceLog);
        }

        // POST: PracticeLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var practiceLog = await _context.PracticeLogs.FindAsync(id);
            if (practiceLog != null)
            {
                _context.PracticeLogs.Remove(practiceLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracticeLogExists(int id)
        {
            return _context.PracticeLogs.Any(e => e.Id == id);
        }
    }
}
