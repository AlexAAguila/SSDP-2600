//using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;
//using WildPath.Models;

//namespace WildPath.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using WildPath.Models;

namespace SSDForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly WildPathDbContext _context;
        public HomeController(WildPathDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Profiles);
        }
    }
}
