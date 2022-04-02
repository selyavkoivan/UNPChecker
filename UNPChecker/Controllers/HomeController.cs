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

        [HttpPost]
        public async Task<IActionResult> AddUnp(string email, string unp)
        {
            await using var context = new ApplicationContext();
            await context.users.AddAsync(new UNP(Convert.ToUInt32(unp), email));
            await context.SaveChangesAsync();
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Context {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}