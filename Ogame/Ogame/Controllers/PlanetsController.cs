using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ogame.Data;
using Ogame.Models;
using System.Security.Claims;

namespace Ogame.Controllers
{
    [Authorize]
    public class PlanetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PlanetsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Planets
        public async Task<IActionResult> Index()
        {
            User user = await GetCurrentUserAsync();
            var applicationDbContext = user.IsAdmin ? _context.Planets.Include(p => p.User) : _context.Planets.Where(p => p.UserID == user.Id).Include(p => p.User);

            if (!_context.Planets.Any(e => e.UserID == user.Id))
            {
                Planet planet = new Planet {
                    Name = "Solaris", Cristal = 15000, Deuterium = 5000, Dist_to_star = 7000, Metal = 15000, Energy = 100000, UserID = user.Id, X = 15, Y = 15
                    // FIXME - Randomize name, and set better values
                };
                await Create(planet);
            }

            return View(new Models.PlanetView.PlanetIndexViewInterface(await applicationDbContext.ToListAsync(), user));
        }

        // GET: Planets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var applicationDbContext = _context.Planets.Include(p => p.User);

            var planet = await _context.Planets
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PlanetID == id);
            if (planet == null)
            {
                return NotFound();
            }

            return View(new Models.PlanetView.PlanetDetailsViewInterface(planet, await GetCurrentUserAsync()));
        }

        // GET: Planets/Create
        public async Task<IActionResult> Create()
        {
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Planets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanetID,UserID,Name,Dist_to_star,X,Y,Metal,Cristal,Deuterium,Energy")] Planet planet)
        {
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(planet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", planet.UserID);
            return View(planet);
        }

        // GET: Planets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            var planet = await _context.Planets.FindAsync(id);
            if (planet == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", planet.UserID);
            return View(planet);
        }

        // POST: Planets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanetID,UserID,Name,Dist_to_star,X,Y,Metal,Cristal,Deuterium,Energy")] Planet planet)
        {
            if (id != planet.PlanetID)
            {
                return NotFound();
            }

            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanetExists(planet.PlanetID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", planet.UserID);
            return View(planet);
        }

        // GET: Planets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            var planet = await _context.Planets
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.PlanetID == id);

            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        // POST: Planets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            var planet = await _context.Planets.FindAsync(id);
            _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanetExists(int id)
        {
            return _context.Planets.Any(e => e.PlanetID == id);
        }
        
        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(_userManager.GetUserId(User));
        }
    }
}
