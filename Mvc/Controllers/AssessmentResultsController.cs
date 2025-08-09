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
    public class AssessmentResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssessmentResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssessmentResults
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AssessmentResults.Include(a => a.Assessment).Include(a => a.Vice);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AssessmentResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentResult = await _context.AssessmentResults
                .Include(a => a.Assessment)
                .Include(a => a.Vice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessmentResult == null)
            {
                return NotFound();
            }

            return View(assessmentResult);
        }

        // GET: AssessmentResults/Create
        public IActionResult Create()
        {
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id");
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id");
            return View();
        }

        // POST: AssessmentResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TotalScore,AssessmentId,ViceId")] AssessmentResult assessmentResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessmentResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", assessmentResult.AssessmentId);
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id", assessmentResult.ViceId);
            return View(assessmentResult);
        }

        // GET: AssessmentResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentResult = await _context.AssessmentResults.FindAsync(id);
            if (assessmentResult == null)
            {
                return NotFound();
            }
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", assessmentResult.AssessmentId);
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id", assessmentResult.ViceId);
            return View(assessmentResult);
        }

        // POST: AssessmentResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalScore,AssessmentId,ViceId")] AssessmentResult assessmentResult)
        {
            if (id != assessmentResult.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessmentResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentResultExists(assessmentResult.Id))
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
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", assessmentResult.AssessmentId);
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id", assessmentResult.ViceId);
            return View(assessmentResult);
        }

        // GET: AssessmentResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentResult = await _context.AssessmentResults
                .Include(a => a.Assessment)
                .Include(a => a.Vice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessmentResult == null)
            {
                return NotFound();
            }

            return View(assessmentResult);
        }

        // POST: AssessmentResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessmentResult = await _context.AssessmentResults.FindAsync(id);
            if (assessmentResult != null)
            {
                _context.AssessmentResults.Remove(assessmentResult);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentResultExists(int id)
        {
            return _context.AssessmentResults.Any(e => e.Id == id);
        }
    }
}
