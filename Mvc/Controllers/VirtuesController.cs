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
    public class VirtuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VirtuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Virtues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Virtues.ToListAsync());
        }

        // GET: Virtues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var virtue = await _context.Virtues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virtue == null)
            {
                return NotFound();
            }

            return View(virtue);
        }

        // GET: Virtues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Virtues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Virtue virtue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(virtue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(virtue);
        }

        // GET: Virtues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var virtue = await _context.Virtues.FindAsync(id);
            if (virtue == null)
            {
                return NotFound();
            }
            return View(virtue);
        }

        // POST: Virtues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Virtue virtue)
        {
            if (id != virtue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(virtue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VirtueExists(virtue.Id))
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
            return View(virtue);
        }

        // GET: Virtues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var virtue = await _context.Virtues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virtue == null)
            {
                return NotFound();
            }

            return View(virtue);
        }

        // POST: Virtues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var virtue = await _context.Virtues.FindAsync(id);
            if (virtue != null)
            {
                _context.Virtues.Remove(virtue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VirtueExists(int id)
        {
            return _context.Virtues.Any(e => e.Id == id);
        }
    }
}
