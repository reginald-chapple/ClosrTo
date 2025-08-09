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
    public class VicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vices.Include(v => v.CorrespondingVirtue);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vice = await _context.Vices
                .Include(v => v.CorrespondingVirtue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vice == null)
            {
                return NotFound();
            }

            return View(vice);
        }

        // GET: Vices/Create
        public IActionResult Create()
        {
            ViewData["CorrespondingVirtueId"] = new SelectList(_context.Virtues, "Id", "Id");
            return View();
        }

        // POST: Vices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CorrespondingVirtueId")] Vice vice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CorrespondingVirtueId"] = new SelectList(_context.Virtues, "Id", "Id", vice.CorrespondingVirtueId);
            return View(vice);
        }

        // GET: Vices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vice = await _context.Vices.FindAsync(id);
            if (vice == null)
            {
                return NotFound();
            }
            ViewData["CorrespondingVirtueId"] = new SelectList(_context.Virtues, "Id", "Id", vice.CorrespondingVirtueId);
            return View(vice);
        }

        // POST: Vices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CorrespondingVirtueId")] Vice vice)
        {
            if (id != vice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViceExists(vice.Id))
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
            ViewData["CorrespondingVirtueId"] = new SelectList(_context.Virtues, "Id", "Id", vice.CorrespondingVirtueId);
            return View(vice);
        }

        // GET: Vices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vice = await _context.Vices
                .Include(v => v.CorrespondingVirtue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vice == null)
            {
                return NotFound();
            }

            return View(vice);
        }

        // POST: Vices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vice = await _context.Vices.FindAsync(id);
            if (vice != null)
            {
                _context.Vices.Remove(vice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViceExists(int id)
        {
            return _context.Vices.Any(e => e.Id == id);
        }
    }
}
