using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogame.Data;
using Ogame.Models;

namespace Ogame.Controllers
{
    public class TemporalActionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemporalActionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TemporalActions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Actions.Include(t => t.Target);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TemporalActions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temporalAction = await _context.Actions
                .Include(t => t.Target)
                .FirstOrDefaultAsync(m => m.TemporalActionID == id);
            if (temporalAction == null)
            {
                return NotFound();
            }

            return View(temporalAction);
        }

        // GET: TemporalActions/Create
        public IActionResult Create()
        {
            ViewData["TargetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID");
            return View();
        }

        // POST: TemporalActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TemporalActionID,Due_to,Type,TargetID")] TemporalAction temporalAction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(temporalAction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TargetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", temporalAction.TargetID);
            return View(temporalAction);
        }

        // GET: TemporalActions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temporalAction = await _context.Actions.FindAsync(id);
            if (temporalAction == null)
            {
                return NotFound();
            }
            ViewData["TargetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", temporalAction.TargetID);
            return View(temporalAction);
        }

        // POST: TemporalActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TemporalActionID,Due_to,Type,TargetID")] TemporalAction temporalAction)
        {
            if (id != temporalAction.TemporalActionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(temporalAction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemporalActionExists(temporalAction.TemporalActionID))
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
            ViewData["TargetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", temporalAction.TargetID);
            return View(temporalAction);
        }

        // GET: TemporalActions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var temporalAction = await _context.Actions
                .Include(t => t.Target)
                .FirstOrDefaultAsync(m => m.TemporalActionID == id);
            if (temporalAction == null)
            {
                return NotFound();
            }

            return View(temporalAction);
        }

        // POST: TemporalActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var temporalAction = await _context.Actions.FindAsync(id);
            _context.Actions.Remove(temporalAction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemporalActionExists(int id)
        {
            return _context.Actions.Any(e => e.TemporalActionID == id);
        }
    }
}
