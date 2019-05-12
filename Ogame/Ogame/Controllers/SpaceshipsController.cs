using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogame.Data;
using Ogame.Models;

namespace Ogame.Controllers
{
    [Authorize]
    public class SpaceshipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpaceshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Spaceships
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Spaceships.Include(s => s.Action).Include(s => s.Caps).Include(s => s.Planet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Spaceships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships
                .Include(s => s.Action)
                .Include(s => s.Caps)
                .Include(s => s.Planet)
                .FirstOrDefaultAsync(m => m.SpaceshipID == id);
            if (spaceship == null)
            {
                return NotFound();
            }

            return View(spaceship);
        }

        // GET: Spaceships/Create
        public IActionResult Create()
        {
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID");
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID");
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID");
            return View();
        }

        // POST: Spaceships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpaceshipID,PlanetID,CapsID,ActionID,Level,Energy")] Spaceship spaceship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spaceship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", spaceship.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", spaceship.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", spaceship.PlanetID);
            return View(spaceship);
        }

        // GET: Spaceships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships.FindAsync(id);
            if (spaceship == null)
            {
                return NotFound();
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", spaceship.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", spaceship.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", spaceship.PlanetID);
            return View(spaceship);
        }

        // POST: Spaceships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpaceshipID,PlanetID,CapsID,ActionID,Level,Energy")] Spaceship spaceship)
        {
            if (id != spaceship.SpaceshipID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spaceship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpaceshipExists(spaceship.SpaceshipID))
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
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", spaceship.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", spaceship.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", spaceship.PlanetID);
            return View(spaceship);
        }

        // GET: Spaceships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships
                .Include(s => s.Action)
                .Include(s => s.Caps)
                .Include(s => s.Planet)
                .FirstOrDefaultAsync(m => m.SpaceshipID == id);
            if (spaceship == null)
            {
                return NotFound();
            }

            return View(spaceship);
        }

        // POST: Spaceships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spaceship = await _context.Spaceships.FindAsync(id);
            _context.Spaceships.Remove(spaceship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpaceshipExists(int id)
        {
            return _context.Spaceships.Any(e => e.SpaceshipID == id);
        }
    }
}
