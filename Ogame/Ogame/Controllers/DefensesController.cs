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
    public class DefensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DefensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Defenses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Defenses.Include(d => d.Action).Include(d => d.Caps).Include(d => d.Planet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Defenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defense = await _context.Defenses
                .Include(d => d.Action)
                .Include(d => d.Caps)
                .Include(d => d.Planet)
                .FirstOrDefaultAsync(m => m.DefenseID == id);
            if (defense == null)
            {
                return NotFound();
            }

            return View(defense);
        }

        // GET: Defenses/Create
        public IActionResult Create()
        {
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID");
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID");
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID");
            return View();
        }

        // POST: Defenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DefenseID,PlanetID,Level,Energy,CapsID,ActionID")] Defense defense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(defense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", defense.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", defense.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", defense.PlanetID);
            return View(defense);
        }

        // GET: Defenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defense = await _context.Defenses.FindAsync(id);
            if (defense == null)
            {
                return NotFound();
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", defense.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", defense.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", defense.PlanetID);
            return View(defense);
        }

        // POST: Defenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DefenseID,PlanetID,Level,Energy,CapsID,ActionID")] Defense defense)
        {
            if (id != defense.DefenseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(defense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DefenseExists(defense.DefenseID))
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
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", defense.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", defense.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", defense.PlanetID);
            return View(defense);
        }

        // GET: Defenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var defense = await _context.Defenses
                .Include(d => d.Action)
                .Include(d => d.Caps)
                .Include(d => d.Planet)
                .FirstOrDefaultAsync(m => m.DefenseID == id);
            if (defense == null)
            {
                return NotFound();
            }

            return View(defense);
        }

        // POST: Defenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var defense = await _context.Defenses.FindAsync(id);
            _context.Defenses.Remove(defense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DefenseExists(int id)
        {
            return _context.Defenses.Any(e => e.DefenseID == id);
        }
    }
}
