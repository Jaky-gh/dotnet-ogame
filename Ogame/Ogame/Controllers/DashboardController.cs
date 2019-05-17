using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogame.Data;
using Ogame.Models;

namespace Ogame.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Dashboard/
        public async Task<IActionResult> Index()
        {
            User user = await GetCurrentUserAsync();
            TemporalActionResolver.HandleTemoralActionForUserUntil(_context, user.Id);
            var applicationDbContext = user.IsAdmin ? _context.Planets.Include(p => p.User) : _context.Planets.Where(p => p.UserID == user.Id).Include(p => p.User);

            if (!_context.Planets.Any(e => e.UserID == user.Id))
            {
                Planet planet = await PlanetRandomizer.FindPlanetForNewPlayer(_context);
                planet.User = user;
                if (planet.PlanetID != 0)
                {
                    _context.Planets.Update(planet);
                    
                }
                else
                {
                    _context.Planets.Add(planet);
                }
                DefaultElementsGenerator.CreateDefaultSpaceship(_context, planet);
                DefaultElementsGenerator.CreateDefaultMine(_context, Mine.Ressources.Metal, planet);
                DefaultElementsGenerator.CreateDefaultMine(_context, Mine.Ressources.Cristal, planet);
                DefaultElementsGenerator.CreateDefaultMine(_context, Mine.Ressources.Deuterium, planet);
                DefaultElementsGenerator.CreateDefaultSolarPanel(_context, planet);
                DefaultElementsGenerator.CreateDefaultDefense(_context, planet);
                await _context.SaveChangesAsync();
            }

            var planets = await applicationDbContext.ToListAsync();
            ViewData["planets"] = planets;
            ViewData["user"] = user;

            return View();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(_userManager.GetUserId(User));
        }
    }
}
