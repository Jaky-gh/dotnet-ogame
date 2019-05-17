using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public SpaceshipsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Spaceships
        public async Task<IActionResult> Index()
        {
            User user = await GetCurrentUserAsync();
            TemporalActionResolver.HandleTemoralActionForUserUntil(_context, user.Id);
            var applicationDbContext = user.IsAdmin ?
                _context.Spaceships.Include(s => s.Action).Include(s => s.Caps).Include(s => s.Planet).Include(s => s.Planet.User) :
                _context.Spaceships.Where(p => p.Planet.UserID == user.Id).Include(s => s.Action).Include(s => s.Caps).Include(s => s.Planet).Include(s => s.Planet.User)
                ;

            return View(new Models.SpaceshipView.SpaceshipIndexViewInterface(await applicationDbContext.ToListAsync(), user));
        }

        // GET: Spaceships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                TemporalActionResolver.HandleTemoralActionForUserUntil(_context, userId);
            }
            if (id == null)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships
                .Include(s => s.Action)
                .Include(s => s.Caps)
                .Include(s => s.Planet)
                .FirstOrDefaultAsync(m => m.SpaceshipID == id);

            User user = await GetCurrentUserAsync();
            if (spaceship == null || (!user.IsAdmin && spaceship.Planet.UserID != user.Id))
            {
                return NotFound();
            }
            return View(new Models.SpaceshipView.SpaceshipDetailsViewInterface(spaceship, user));
        }

        // GET: Spaceships/Create
        public async Task<IActionResult> Create()
        {
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

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
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

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
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

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
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

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
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

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
            User user = await GetCurrentUserAsync();
            if (!user.IsAdmin)
            {
                return NotFound();
            }

            var spaceship = await _context.Spaceships.FindAsync(id);
            _context.Spaceships.Remove(spaceship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Spaceships/Attack/5
        public async Task<IActionResult> Attack(int? id)
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

            Random rnd = new Random();

            var dist = rnd.Next(1, (int) (spaceship.Energy / 10) + 1);
            var x = rnd.Next(0, dist + 1);
            var y = dist - x;

            x *= rnd.Next(0, 2) == 1 ? 1 : -1;
            y *= rnd.Next(0, 2) == 1 ? 1 : -1;

            ViewData["planet"] = await PlanetRandomizer.GetExistingOrRandomPlanet(_context, x, y);
            ViewData["user"] = await GetCurrentUserAsync();
            ViewData["spaceshipPlanetId"] = spaceship.PlanetID;
            ViewData["distance"] = dist;
            ViewData["energyCost"] = 10; //FIXME

            return View(new Models.SpaceshipView.SpaceshipAttackInterface());
        }

        // POST: Spaceships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attack(int id, [Bind("_X,_Y")] Models.SpaceshipView.SpaceshipAttackInterface spaceshipAttackInterface)
        {
            Spaceship spaceship = _context.Spaceships
                .Include(s => s.Action)
                .Include(s => s.Action.Target)
                .FirstOrDefault(s => s.SpaceshipID == id);
            if (spaceship == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await VesselAttackHandler.AttackWithSpaceship(_context, spaceship, spaceshipAttackInterface._X, spaceshipAttackInterface._Y);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpaceshipExists(spaceship.SpaceshipID))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionID"] = new SelectList(_context.Actions, "TemporalActionID", "TemporalActionID", spaceship.ActionID);
            ViewData["CapsID"] = new SelectList(_context.Caps, "CapsID", "CapsID", spaceship.CapsID);
            ViewData["PlanetID"] = new SelectList(_context.Planets, "PlanetID", "PlanetID", spaceship.PlanetID);
            return View(new Models.SpaceshipView.SpaceshipAttackInterface());
        }

        private bool SpaceshipExists(int id)
        {
            return _context.Spaceships.Any(e => e.SpaceshipID == id);
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(_userManager.GetUserId(User));
        }
    }
}
