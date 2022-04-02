using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UNPChecker.Models;

namespace UNPChecker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CheckUNP(string UNP)
        {
            await using var context = new ApplicationContext();
            var user = await context.users.FirstOrDefaultAsync(u => u.UNPCode.ToString().Equals(UNP));
            return user == default(UNP) ? Error() : Ok();
        }

        public IActionResult Index()
        {
            
            using var context = new ApplicationContext();
            var users = context.users.ToList();
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Context { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}