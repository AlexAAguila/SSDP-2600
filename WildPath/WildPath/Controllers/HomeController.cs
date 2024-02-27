using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WildPath.EfModels;
using WildPath.Models;
using WildPath.Repositories;

namespace WildPath.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WildPathDbContext _wpdb;

        public HomeController(ILogger<HomeController> logger, WildPathDbContext wpdp)
        {
            _logger = logger;
            _wpdb = wpdp;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PayPalConfirmation(PayPalConfirmationModel payPalConfirmationModel)
        {
            var transRepo = new TransactionRepo(_wpdb);
            transRepo.Add(payPalConfirmationModel);
            return View(payPalConfirmationModel);
        }

        public IActionResult CustomerCare()
        {
            return View();
        }

        public IActionResult TermsAndConditions()
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
