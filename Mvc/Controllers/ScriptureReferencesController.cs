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
    public class ScriptureReferencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScriptureReferencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScriptureReferences
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScriptureReferences.ToListAsync());
        }

        // GET: ScriptureReferences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptureReference = await _context.ScriptureReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scriptureReference == null)
            {
                return NotFound();
            }

            return View(scriptureReference);
        }

        // GET: ScriptureReferences/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScriptureReferences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Book,Chapter,Verses,Text")] ScriptureReference scriptureReference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scriptureReference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scriptureReference);
        }

        // GET: ScriptureReferences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptureReference = await _context.ScriptureReferences.FindAsync(id);
            if (scriptureReference == null)
            {
                return NotFound();
            }
            return View(scriptureReference);
        }

        // POST: ScriptureReferences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Book,Chapter,Verses,Text")] ScriptureReference scriptureReference)
        {
            if (id != scriptureReference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scriptureReference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScriptureReferenceExists(scriptureReference.Id))
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
            return View(scriptureReference);
        }

        // GET: ScriptureReferences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scriptureReference = await _context.ScriptureReferences
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scriptureReference == null)
            {
                return NotFound();
            }

            return View(scriptureReference);
        }

        // POST: ScriptureReferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scriptureReference = await _context.ScriptureReferences.FindAsync(id);
            if (scriptureReference != null)
            {
                _context.ScriptureReferences.Remove(scriptureReference);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScriptureReferenceExists(int id)
        {
            return _context.ScriptureReferences.Any(e => e.Id == id);
        }
    }
}
