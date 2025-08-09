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
    public class AssessmentResponsesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssessmentResponsesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssessmentResponses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AssessmentResponses.Include(a => a.Assessment).Include(a => a.AssessmentQuestion);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AssessmentResponses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentResponse = await _context.AssessmentResponses
                .Include(a => a.Assessment)
                .Include(a => a.AssessmentQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessmentResponse == null)
            {
                return NotFound();
            }

            return View(assessmentResponse);
        }

        // GET: AssessmentResponses/Create
        public IActionResult Create()
        {
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id");
            ViewData["AssessmentQuestionId"] = new SelectList(_context.AssessmentQuestions, "Id", "Id");
            return View();
        }

        // POST: AssessmentResponses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score,AssessmentId,AssessmentQuestionId")] AssessmentResponse assessmentResponse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessmentResponse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", assessmentResponse.AssessmentId);
            ViewData["AssessmentQuestionId"] = new SelectList(_context.AssessmentQuestions, "Id", "Id", assessmentResponse.AssessmentQuestionId);
            return View(assessmentResponse);
        }

        // GET: AssessmentResponses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentResponse = await _context.AssessmentResponses.FindAsync(id);
            if (assessmentResponse == null)
            {
                return NotFound();
            }
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", assessmentResponse.AssessmentId);
            ViewData["AssessmentQuestionId"] = new SelectList(_context.AssessmentQuestions, "Id", "Id", assessmentResponse.AssessmentQuestionId);
            return View(assessmentResponse);
        }

        // POST: AssessmentResponses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Score,AssessmentId,AssessmentQuestionId")] AssessmentResponse assessmentResponse)
        {
            if (id != assessmentResponse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessmentResponse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentResponseExists(assessmentResponse.Id))
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
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "Id", "Id", assessmentResponse.AssessmentId);
            ViewData["AssessmentQuestionId"] = new SelectList(_context.AssessmentQuestions, "Id", "Id", assessmentResponse.AssessmentQuestionId);
            return View(assessmentResponse);
        }

        // GET: AssessmentResponses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentResponse = await _context.AssessmentResponses
                .Include(a => a.Assessment)
                .Include(a => a.AssessmentQuestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessmentResponse == null)
            {
                return NotFound();
            }

            return View(assessmentResponse);
        }

        // POST: AssessmentResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessmentResponse = await _context.AssessmentResponses.FindAsync(id);
            if (assessmentResponse != null)
            {
                _context.AssessmentResponses.Remove(assessmentResponse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentResponseExists(int id)
        {
            return _context.AssessmentResponses.Any(e => e.Id == id);
        }
    }
}
