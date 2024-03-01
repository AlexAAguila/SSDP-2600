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

        public JsonResult AddToCart(int id)
        {

            string cartSession = HttpContext.Session.GetString("Cart");

            if (cartSession != null)
            {

                List<CartItem> cartItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cartSession);

                if (cartItems.Any(c => c.Id == id))
                {                   
                    CartItem cartItem = cartItems.FirstOrDefault(c => c.Id == id);

                    int index = cartItems.FindIndex(c => c.Id == id);

                    if(index != -1)
                    {
                        cartItems[index] = new CartItem
                        {
                            Id = id,
                            Quantity = cartItem.Quantity + 1
                        };
                    }
                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));
                }
                else
                {

                    cartItems.Add(new CartItem
                    {
                        Id = id,
                        Quantity = 1
                    });
                }
            }
            else
            {
                List<CartItem> cartItems = new List<CartItem> { new CartItem { Id = id,
                                                                               Quantity = 1 } };


              HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));
                cartSession = HttpContext.Session.GetString("Cart");
            }
            return Json("Success");
        }

        public JsonResult RemoveToCart(int id)
        {

            return Json("");
        }
    }
}
