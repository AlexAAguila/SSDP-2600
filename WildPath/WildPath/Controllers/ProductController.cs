using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Details(int id)
        {
            ProductRepo productRepo = new ProductRepo(_wpdb);
            return View(productRepo.GetById(id));
        }


    }
}
