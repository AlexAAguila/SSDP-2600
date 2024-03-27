﻿using Microsoft.AspNetCore.Authorization;
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
                if (isLoggedIn)
                {
                    var addresses = transactionRepo.GetAddress(User.Identity.Name);
                    var address = addresses.FirstOrDefault();
                    if (address != null)
                    {
                        model = new CheckoutVM
                        {
                            IsLoggedIn = isLoggedIn,
                            hasAddress = true,
                            totalPrice = totalPrice,
                            totalItems = totalItems,
               
                            Address = address
                            
                        };
                    }
                    else
                    {
                        model = new CheckoutVM
                        {
                            IsLoggedIn = isLoggedIn,
                            hasAddress = false,
                            totalPrice = totalPrice,
                            totalItems = totalItems
                        };
                    }
                }

                else
                {
                     model = new CheckoutVM
                    {
                        IsLoggedIn = isLoggedIn,
                        hasAddress = false,
                        totalPrice = totalPrice,
                        totalItems = totalItems
                    };
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
