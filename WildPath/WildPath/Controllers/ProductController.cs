using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SendGrid.Helpers.Mail;
using WildPath.EfModels;
using WildPath.Models;
using WildPath.Repositories;

namespace WildPath.Controllers
{
    public class ProductController : Controller
    {
        private readonly WildPathDbContext _wpdb;

        public ProductController(WildPathDbContext wpdb)
        {
            _wpdb = wpdb;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetAll());
        }

        public IActionResult ShopAll(string sortOrder, string searchString, int? pageNumber)
        {
            ViewData["currentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            IQueryable<Item> items = _wpdb.Items;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(p => p.ItemName.Contains(searchString) ||
                                         p.Supplier.Contains(searchString) ||
                                         p.Category.Contains(searchString) ||
                                         p.Weight.ToString().Contains(searchString) ||
                                         p.Size.Contains(searchString) ||
                                         p.Colour.Contains(searchString));
            }

            switch (sortOrder)
            {
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
                    items = items.OrderBy(p => p.ItemName);
                    break;
            }

            int pageSize = 4;
            int pageIndex = pageNumber ?? 1;
            var count = items.Count();
            var paginatedItems = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var paginatedItemsModel = new PaginatedList<Item>(paginatedItems, count, pageIndex, pageSize);

            return View(paginatedItemsModel);
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
