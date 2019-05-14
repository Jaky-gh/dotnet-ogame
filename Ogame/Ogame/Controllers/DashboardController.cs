using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ogame.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // GET: /Dashboard/
        public IActionResult Index()
        {
            return View();
        }
    }
}
