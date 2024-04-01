using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WildPath.Data;
using WildPath.Repositories;
using WildPath.EfModels;
using WildPath.ViewModels;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WildPath.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly WildPathDbContext _wpdb;
        private readonly TransactionRepo _transactionRepo;
        public TransactionController(ILogger<HomeController> logger, IConfiguration configuration, WildPathDbContext wpdb, TransactionRepo transactionRepo)
        {
            _logger = logger;
            _configuration = configuration;
            _wpdb = wpdb;
            _transactionRepo = transactionRepo;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            TransactionRepo transactionRepo = new TransactionRepo(_wpdb);

            return View(transactionRepo.GetAll());
        }

        [Authorize]
        public IActionResult UserTransactions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userTransactions = _wpdb.Transactions
                .Include(t => t.FkAddress)
                .Where(t => t.FkUserId == userId)
                .ToList();

            return View(userTransactions);
        }


        public JsonResult AddToCart(int id, int quantity)
        {
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems;

            if (cartSession != null)
            {
                cartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
            }
            else
            {
                cartItems = new List<CartItem>();
            }

            CartItem cartItem = cartItems.FirstOrDefault(c => c.Id == id);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity; // Increment the quantity by the specified amount
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    Id = id,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));

            return Json("Success");
        }


        public JsonResult RemoveOneFromCart(int id)
        {
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems;

            if (cartSession != null)
            {
                cartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
            }
            else
            {
                // If there's no cart, then there's nothing to remove from, return an error or a default response
                return Json("No items in cart.");
            }

            CartItem cartItem = cartItems.FirstOrDefault(c => c.Id == id);

            if (cartItem != null)
            {
                cartItem.Quantity -= 1; // Decrement the quantity by one

                // If quantity is zero or less, consider removing the item from the cart
                if (cartItem.Quantity <= 0)
                {
                    cartItems.Remove(cartItem);
                }

                // Update the session with the modified cart
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));
                return Json("Item removed successfully.");
            }
            else
            {
                // If the item wasn't found in the cart, return an appropriate response
                return Json("Item not found in cart.");
            }
        }










        public JsonResult RemoveToCart(int id)
        {
            // Retrieve cart items from session
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = new List<CartItem>();

            if (!string.IsNullOrEmpty(cartSession))
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartSession);
            }

            // Find and remove the item with the specified ID
            CartItem itemToRemove = cartItems.FirstOrDefault(c => c.Id == id);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }

            // Update session with modified cart
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));

            // Return success message
            return Json(new { success = true, message = "Item removed successfully" });
        }

        public IActionResult GetCartState()
        {
            // Retrieve cart items from session
            string cartSession = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems = new List<CartItem>();

            if (!string.IsNullOrEmpty(cartSession))
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartSession);
            }

            // Return cart items as JSON
            return Json(cartItems);
        }
        public IActionResult Checkout()
        {
            const string CURRENCY_CODE = "CAD";           //Should be pulled from the database!
            const string CURRENCY_SYMBOL = "$";           //Should be pulled from the database!
            const double CA_TAX = .12;                    //Should be pulled from the database!
            const double SHIPPING_RATE = 7.99;            //Should be pulled from the database!
            const double FREE_SHIPPING_THRESHOLD = 74.99; //Should be pulled from the database!

            List<CartItemVM> cartItems = new List<CartItemVM>();

            var isLoggedIn = User.Identity.IsAuthenticated;

            string cartSession = HttpContext.Session.GetString("Cart");

            if (cartSession != null)
            {
                List<CartItem> sessionCartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
                ProductRepo productRepo = new ProductRepo(_wpdb);
                TransactionRepo transactionRepo = new TransactionRepo(_wpdb);
                cartItems = productRepo.GetCartVM(sessionCartItems);

                double totalPrice = 0;
                int totalItems = 0;
                CheckoutVM model;

                foreach (var item in cartItems)
                {
                    totalPrice += item.Price * item.Quantity;
                    totalItems += item.Quantity;
                }

                double shipping = totalPrice > FREE_SHIPPING_THRESHOLD ? 0 : SHIPPING_RATE;
                double tax = totalPrice * CA_TAX;
                double grandTotal = totalPrice + shipping + tax;

                model = new CheckoutVM
                {
                    IsLoggedIn = isLoggedIn,
                    hasAddress = false,
                    Address = new Address(),
                    TotalItems = totalItems,
                    TotalPrice = totalPrice,
                    Shipping = shipping,
                    ShippingRate = SHIPPING_RATE,
                    ShippingMethod = shipping == 0 ? "Standard Shipping" : "Express Shipping",
                    FreeShippingThreshold = FREE_SHIPPING_THRESHOLD,
                    IsFreeShipping = shipping == 0 ? true : false,
                    Tax = tax,
                    GrandTotal = grandTotal,
                    CurrencyCode = CURRENCY_CODE,
                    CurrencySymbol = CURRENCY_SYMBOL,
                };

                if (isLoggedIn)
                {
                    //model.Email = User.Identity.Name;
                    var addresses = transactionRepo.GetAddress(User.Identity.Name);
                    Address address = new Address();

                    if (addresses.Count() > 0)
                    {
                        model.hasAddress = true;

                        address = addresses.FirstOrDefault();
                        model.Address = address;
                    }
                }

                return View(model);
            }
            else
            {
                return View();
            }
        }

    }
}
