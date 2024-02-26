using Microsoft.AspNetCore.Mvc;
using WildPath.Data;
using WildPath.Repositories;
using WildPath.EfModels;

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
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_wpdb);


            TransactionRepo transactionRepo = new TransactionRepo(_wpdb);




            return View(transactionRepo.GetAll());

        }
    }
}
