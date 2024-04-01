using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WildPath.EfModels;
using WildPath.Models;
using WildPath.Repositories;
using WildPath.ViewModels;

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

        [HttpPost]
        public IActionResult ProcessTransaction(CheckoutVM CheckoutVM)
        {
            CheckoutVM.TrackingNumber = GenerateTrackingNumber();

            CheckoutVM.EstimatedDeliveryDate = DateTime.Now.AddDays(14);

            var transRepo = new TransactionRepo(_wpdb);

            transRepo.Add(CheckoutVM);

            return RedirectToAction("PayPalConfirmation", CheckoutVM);
        }

        public IActionResult PayPalConfirmation(CheckoutVM CheckoutVM)
        {
            string cartSession = HttpContext.Session.GetString("Cart");

            if (cartSession != null)
            {
                List<CartItem> sessionCartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
                sessionCartItems.Clear();

                // Serialize and update the session
                HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(sessionCartItems));
            }

            return View(CheckoutVM);
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
