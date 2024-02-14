using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SendGrid.Helpers.Mail;
using WildPath.EfModels;
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

        public IActionResult Index()
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetAll());
        }

        public IActionResult Category(string category)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);

            return View(productRepo.GetByCategory(category));
        }

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
