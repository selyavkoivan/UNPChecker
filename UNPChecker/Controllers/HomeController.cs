using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using UNPChecker.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UNPChecker.Controllers
{
    public class HomeController : Controller
    {
        private static Timer timer;
        private static object synclock = new object();
        private static bool sent = false;
        private const long interval = 30000;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            timer = new Timer(SendEmail, null, 0, interval);
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

        private void SendEmail(object obj)
        {
            lock (synclock)
            {
                var dd = DateTime.Now;
                if (dd.Hour == 7 && dd.Minute == 0 && sent == false)
                {
                    sent = true;
                    using var context = new ApplicationContext();
                    var users = context.users.ToList();
                    if (users.Count > 100)
                    {
                        var firstPart = users.Take(100).ToList();
                        var secondPart = users.ToArray().Except(firstPart).ToList();
                        EmailService.SendEmail(firstPart);
                        Thread.Sleep(1000 * 60*5);
                        EmailService.SendEmail(secondPart);
                    }
                    else EmailService.SendEmail(users);
                }
                else if (dd.Hour != 1 && dd.Minute != 30)
                {
                    sent = false;
                }
            }
        }
    }
}