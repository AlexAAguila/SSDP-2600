using Microsoft.AspNetCore.Mvc;
using WildPath.Data;
using WildPath.Repositories;
using WildPath.EfModels;
using WildPath.ViewModels;
using System.Text.Json;

namespace WildPath.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly WildPathDbContext _wpdb;
        public TransactionController(ILogger<HomeController> logger, IConfiguration configuration, WildPathDbContext wpdb)
        {
            _logger = logger;
            _configuration = configuration;
            _wpdb = wpdb;
        }

        public IActionResult Index()
        {
            //MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);


            TransactionRepo transactionRepo = new TransactionRepo(_wpdb);




            return View(transactionRepo.GetAll());

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
            List<CartItemVM> cartItems = new List<CartItemVM>();

            var isLoggedIn = User.Identity.IsAuthenticated;

            string cartSession = HttpContext.Session.GetString("Cart");

            if (cartSession != null)
            {
                List<CartItem> sessionCartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);
                ProductRepo productRepo = new ProductRepo(_wpdb);
                cartItems = productRepo.GetCartVM(sessionCartItems);

                double totalPrice = 0;
                int totalItems = 0;

                foreach (var item in cartItems)
                {
                    totalPrice += item.Price * item.Quantity;
                    totalItems += item.Quantity;
                }
               

                var model = new CheckoutVM
                {
                    IsLoggedIn = isLoggedIn,
                    totalPrice = totalPrice,
                    totalItems = totalItems
                };


                return View(model);
            }
            else
            {
                return View();
            }
        }

    }
}
