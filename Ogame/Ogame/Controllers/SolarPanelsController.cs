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
    public class SolarPanelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolarPanelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SolarPanels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SolarPanels.Include(s => s.Action).Include(s => s.Caps).Include(s => s.Planet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SolarPanels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solarPanel = await _context.SolarPanels
                .Include(s => s.Action)
                .Include(s => s.Caps)
                .Include(s => s.Planet)
                .FirstOrDefaultAsync(m => m.SolarPanelID == id);
            if (solarPanel == null)
            {
                return NotFound();
            }

            return View(solarPanel);
        }

        // GET: SolarPanels/Create
        public IActionResult Create()
        {
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID");
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID");
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID");
            return View();
        }

        // POST: SolarPanels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SolarPanelID,Level,Collect_rate,PlanetID,CapsID,ActionID")] SolarPanel solarPanel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solarPanel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", solarPanel.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", solarPanel.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", solarPanel.PlanetID);
            return View(solarPanel);
        }

        // GET: SolarPanels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solarPanel = await _context.SolarPanels.FindAsync(id);
            if (solarPanel == null)
            {
                return NotFound();
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", solarPanel.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", solarPanel.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", solarPanel.PlanetID);
            return View(solarPanel);
        }

        // POST: SolarPanels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SolarPanelID,Level,Collect_rate,PlanetID,CapsID,ActionID")] SolarPanel solarPanel)
        {
            if (id != solarPanel.SolarPanelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solarPanel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolarPanelExists(solarPanel.SolarPanelID))
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
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", solarPanel.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", solarPanel.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", solarPanel.PlanetID);
            return View(solarPanel);
        }

        // GET: SolarPanels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solarPanel = await _context.SolarPanels
                .Include(s => s.Action)
                .Include(s => s.Caps)
                .Include(s => s.Planet)
                .FirstOrDefaultAsync(m => m.SolarPanelID == id);
            if (solarPanel == null)
            {
                return NotFound();
            }

            return View(solarPanel);
        }

        // POST: SolarPanels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solarPanel = await _context.SolarPanels.FindAsync(id);
            _context.SolarPanels.Remove(solarPanel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolarPanelExists(int id)
        {
            return _context.SolarPanels.Any(e => e.SolarPanelID == id);
        }
    }
}
