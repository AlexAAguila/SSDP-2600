using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SendGridDemo.Data.Services;
using System.Diagnostics;
using System.Net.Mail;
using WildPath.Data;
using WildPath.EfModels;
using WildPath.Migrations;
using WildPath.Models;
using WildPath.Repositories;
using WildPath.ViewModels;
using WildPath.Data.Services;

namespace WildPath.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _emailService;
        private readonly WildPathDbContext _wpdb;
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ILogger<HomeController> logger, WildPathDbContext wpdp, ApplicationDbContext applicationDbContext, IEmailService emailService
)
        {
            _logger = logger;
            _wpdb = wpdp;
            _applicationDbContext = applicationDbContext;
            _emailService = emailService;

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
        public IActionResult ProcessTransaction(CheckoutVM CheckoutVM)
        {

            CheckoutVM.TrackingNumber = GenerateTrackingNumber();

            CheckoutVM.EstimatedDeliveryDate = DateTime.Now.AddDays(14);

            var transRepo = new TransactionRepo(_wpdb);
            string? userId = null;
            string userName = CheckoutVM.PayerEmail;
            if (User.Identity.IsAuthenticated)
            {
                MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);
                 userName = User.Identity.Name;
                string name = registeredUserRepo.GetUserByName(userName).FirstName;
                ViewBag.UserName = name;
                // Assuming ApplicationDbContext is your Entity Framework DbContext
                userId = _applicationDbContext.Users
                                    .Where(u => u.UserName == User.Identity.Name)
                                    .Select(u => u.Id)
                                    .FirstOrDefault();
                CheckoutVM.PayerFullName = registeredUserRepo.GetFirstAndLastNameByEmail(User.Identity.Name);

                
                registeredUserRepo.AddFkAddress(CheckoutVM, userName);
                
            }
            var rounded = CheckoutVM.GrandTotal.ToString("N2");
            var emailBody = $"Hi {CheckoutVM.PayerFullName} Thank you for your payment of ${rounded} has been processed successfully. Your shipment with Tracking number: #{CheckoutVM.TrackingNumber} is ready. \n\n Delivered to: \n\n{CheckoutVM.Address.Address1} \n{CheckoutVM.Address.City}, {CheckoutVM.Address.PostalCode} \n\n Estimated Delivery Date: {CheckoutVM.EstimatedDeliveryDate}";

            _emailService.SendSingleEmail(new ComposeEmailModel
            {
                Subject = "Transaction Confirmation",
                Body = emailBody,
                Email = User.Identity.Name ?? CheckoutVM.PayerEmail // assuming CheckoutVM.Email is the customer's email
            });
            transRepo.Add(CheckoutVM, userId, userName);

            return RedirectToAction("PayPalConfirmation", CheckoutVM);
        }

        public IActionResult PayPalConfirmation(CheckoutVM CheckoutVM)
        {
            if (User.Identity.IsAuthenticated)
            {
                MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);
                string userName = User.Identity.Name;
                string name = registeredUserRepo.GetUserByName(userName).FirstName;
                ViewBag.UserName = name;
            }
            string cartSession = HttpContext.Session.GetString("Cart");
            TransactionRepo transactionRepo = new TransactionRepo(_wpdb);
            var transId = transactionRepo.GetTransactionsByTransactionId(CheckoutVM.TransactionId);
            CheckoutVM.Address = _wpdb.Addresses.Where(u => u.PkAddressId == transId.FirstOrDefault().FkAddressId)
                                    .FirstOrDefault();

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
            if (User.Identity.IsAuthenticated)
            {
                MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);
                string userName = User.Identity.Name;
                string name = registeredUserRepo.GetUserByName(userName).FirstName;
                ViewBag.UserName = name;
            }
            return View();
        }

        public IActionResult TermsAndConditions()
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

        public IActionResult Privacy()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (User.Identity.IsAuthenticated)
            {
                MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);
                string userName = User.Identity.Name;
                string name = registeredUserRepo.GetUserByName(userName).FirstName;
                ViewBag.UserName = name;
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
