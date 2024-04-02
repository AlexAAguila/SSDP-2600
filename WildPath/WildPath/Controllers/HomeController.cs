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
            if (User.Identity.IsAuthenticated)
            {
                MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);
                string userName = User.Identity.Name;
                string name = registeredUserRepo.GetUserByName(userName).FirstName;
                ViewBag.UserName = name;
            }

            return View();
        }

        [HttpPost]
        public IActionResult ProcessTransaction([FromBody] PayPalConfirmationModel payPalConfirmationModel)
        {
            payPalConfirmationModel.TrackingNumber = GenerateTrackingNumber();

            payPalConfirmationModel.EstimatedDeliveryDate = DateTime.Now.AddDays(14);

            var transRepo = new TransactionRepo(_wpdb);
            //var address = transRepo.AddAddress(payPalConfirmationModel);
            //Console.WriteLine(address);
            transRepo.Add(payPalConfirmationModel);
            return RedirectToAction("PayPalConfirmation", payPalConfirmationModel);
        }


        public IActionResult PayPalConfirmation(PayPalConfirmationModel payPalConfirmationModel)
        {
            return View("PayPalConfirmation", payPalConfirmationModel);
        }
        private string GenerateTrackingNumber()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
