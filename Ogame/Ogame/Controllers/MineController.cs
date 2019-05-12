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
    public class MineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mine
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mines.Include(m => m.Action).Include(m => m.Caps).Include(m => m.Planet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Mine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mine = await _context.Mines
                .Include(m => m.Action)
                .Include(m => m.Caps)
                .Include(m => m.Planet)
                .FirstOrDefaultAsync(m => m.MineID == id);
            if (mine == null)
            {
                return NotFound();
            }

            return View(mine);
        }

        // GET: Mine/Create
        public IActionResult Create()
        {
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID");
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID");
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID");
            return View();
        }

        // POST: Mine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MineID,PlanetID,CapsID,ActionID,Level,Ressource")] Mine mine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", mine.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", mine.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", mine.PlanetID);
            return View(mine);
        }

        // GET: Mine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mine = await _context.Mines.FindAsync(id);
            if (mine == null)
            {
                return NotFound();
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", mine.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", mine.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", mine.PlanetID);
            return View(mine);
        }

        // POST: Mine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MineID,PlanetID,CapsID,ActionID,Level,Ressource")] Mine mine)
        {
            if (id != mine.MineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MineExists(mine.MineID))
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
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", mine.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", mine.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", mine.PlanetID);
            return View(mine);
        }

        // GET: Mine/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mine = await _context.Mines
                .Include(m => m.Action)
                .Include(m => m.Caps)
                .Include(m => m.Planet)
                .FirstOrDefaultAsync(m => m.MineID == id);
            if (mine == null)
            {
                return NotFound();
            }

            return View(mine);
        }

        // POST: Mine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mine = await _context.Mines.FindAsync(id);
            _context.Mines.Remove(mine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MineExists(int id)
        {
            return _context.Mines.Any(e => e.MineID == id);
        }
    }
}
