using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SendGrid.Helpers.Mail;
using WildPath.EfModels;
using WildPath.Repositories;

namespace WildPath.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly WildPathDbContext _wpdb;

        public ProductController(WildPathDbContext wpdb)
        {
            _wpdb = wpdb;
        }

        public IActionResult Index()
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetAll());
        }

        public IActionResult ShopAll(string sortOrder, string searchString)
        {
            ViewData["SupplierSortParm"] = string.IsNullOrEmpty(sortOrder) ? "supplier_desc" : "";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            IQueryable<Item> items = _wpdb.Items;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(p => p.ItemName.Contains(searchString) ||
                                         p.Supplier.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "supplier_desc":
                    items = items.OrderByDescending(p => p.Supplier);
                    break;
                case "Name":
                    items = items.OrderBy(p => p.ItemName);
                    break;
                case "name_desc":
                    items = items.OrderByDescending(p => p.ItemName);
                    break;
                case "Price":
                    items = items.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(p => p.Price);
                    break;
                default:
                    items = items.OrderBy(p => p.Supplier);
                    break;
            }

            return View(items.ToList());
        }


        //public IActionResult Category(string category)
        //{
        //    ViewData["Category"] = category;

        //    ProductRepo productRepo = new ProductRepo(_wpdb);
        //    var products = productRepo.GetByCategory(category);

        //    return View(products);
        //}

        public IActionResult Details(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);
            return View(productRepo.GetById(id));
        }

        public IActionResult Create()
        {
            Item item = new Item();

            return View(item);
        }

        [HttpPost]
        public IActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                ProductRepo productRepo = new ProductRepo(_wpdb);

                string addMessage = productRepo.Add(item);

                return RedirectToAction("Index", new { message = addMessage });
            }

            return View(item);
        }
        public IActionResult Edit(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetById(id));
        }

        [HttpPost]
        public IActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                ProductRepo productRepo = new ProductRepo(_wpdb);

                string repoMessage = productRepo.Update(item);

                return RedirectToAction("Index", new { message = repoMessage });
            }

            return View(item);
        }
        public IActionResult Delete(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetById(id));
        }

        [HttpPost]
        public IActionResult Delete(Item item)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            string repoMessage = productRepo.Delete(item.PkItemId);

            return RedirectToAction("Index", new { message = repoMessage });
        }
    }
}
