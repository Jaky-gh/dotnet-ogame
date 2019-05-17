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
            var applicationDbContext = user.IsAdmin ? _context.Planets.Include(p => p.User) : _context.Planets.Where(p => p.UserID == user.Id).Include(p => p.User);

            ViewData["planets"] = await applicationDbContext.ToListAsync();
            ViewData["user"] = user;
            return View();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(_userManager.GetUserId(User));
        }
    }
}
