using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ogame.Models;
using Ogame.Data;

namespace Ogame.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string id = _userManager.GetUserId(User);
            if (id != null)
            {
                TemporalActionResolver.HandleTemoralActionForUserUntil(_context, id);
            }
            return View();
        }

        public IActionResult About()
        {
            string id = _userManager.GetUserId(User);
            if (id != null)
            {
                TemporalActionResolver.HandleTemoralActionForUserUntil(_context, id);
            }
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            string id = _userManager.GetUserId(User);
            if (id != null)
            {
                TemporalActionResolver.HandleTemoralActionForUserUntil(_context, id);
            }
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            string id = _userManager.GetUserId(User);
            if (id != null)
            {
                TemporalActionResolver.HandleTemoralActionForUserUntil(_context, id);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
