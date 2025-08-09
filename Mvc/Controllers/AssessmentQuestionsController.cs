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
    public class AssessmentQuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssessmentQuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssessmentQuestions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AssessmentQuestions.Include(a => a.Vice);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AssessmentQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentQuestion = await _context.AssessmentQuestions
                .Include(a => a.Vice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessmentQuestion == null)
            {
                return NotFound();
            }

            return View(assessmentQuestion);
        }

        // GET: AssessmentQuestions/Create
        public IActionResult Create()
        {
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id");
            return View();
        }

        // POST: AssessmentQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,ViceId")] AssessmentQuestion assessmentQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assessmentQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id", assessmentQuestion.ViceId);
            return View(assessmentQuestion);
        }

        // GET: AssessmentQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentQuestion = await _context.AssessmentQuestions.FindAsync(id);
            if (assessmentQuestion == null)
            {
                return NotFound();
            }
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id", assessmentQuestion.ViceId);
            return View(assessmentQuestion);
        }

        // POST: AssessmentQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ViceId")] AssessmentQuestion assessmentQuestion)
        {
            if (id != assessmentQuestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessmentQuestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentQuestionExists(assessmentQuestion.Id))
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
            ViewData["ViceId"] = new SelectList(_context.Vices, "Id", "Id", assessmentQuestion.ViceId);
            return View(assessmentQuestion);
        }

        // GET: AssessmentQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessmentQuestion = await _context.AssessmentQuestions
                .Include(a => a.Vice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assessmentQuestion == null)
            {
                return NotFound();
            }

            return View(assessmentQuestion);
        }

        // POST: AssessmentQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessmentQuestion = await _context.AssessmentQuestions.FindAsync(id);
            if (assessmentQuestion != null)
            {
                _context.AssessmentQuestions.Remove(assessmentQuestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentQuestionExists(int id)
        {
            return _context.AssessmentQuestions.Any(e => e.Id == id);
        }
    }
}
