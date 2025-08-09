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
    public class RelationshipGoalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelationshipGoalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RelationshipGoals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RelationshipGoals.Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RelationshipGoals/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationshipGoal = await _context.RelationshipGoals
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relationshipGoal == null)
            {
                return NotFound();
            }

            return View(relationshipGoal);
        }

        // GET: RelationshipGoals/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: RelationshipGoals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Relationship,Name,Goal,ToWhom,Date,CompletedAt,IsCompleted,UserId")] RelationshipGoal relationshipGoal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relationshipGoal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", relationshipGoal.UserId);
            return View(relationshipGoal);
        }

        // GET: RelationshipGoals/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationshipGoal = await _context.RelationshipGoals.FindAsync(id);
            if (relationshipGoal == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", relationshipGoal.UserId);
            return View(relationshipGoal);
        }

        // POST: RelationshipGoals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Relationship,Name,Goal,ToWhom,Date,CompletedAt,IsCompleted,UserId")] RelationshipGoal relationshipGoal)
        {
            if (id != relationshipGoal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relationshipGoal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelationshipGoalExists(relationshipGoal.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", relationshipGoal.UserId);
            return View(relationshipGoal);
        }

        // GET: RelationshipGoals/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationshipGoal = await _context.RelationshipGoals
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relationshipGoal == null)
            {
                return NotFound();
            }

            return View(relationshipGoal);
        }

        // POST: RelationshipGoals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var relationshipGoal = await _context.RelationshipGoals.FindAsync(id);
            if (relationshipGoal != null)
            {
                _context.RelationshipGoals.Remove(relationshipGoal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RelationshipGoalExists(long id)
        {
            return _context.RelationshipGoals.Any(e => e.Id == id);
        }
    }
}
