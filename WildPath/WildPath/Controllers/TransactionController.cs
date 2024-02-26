using Microsoft.AspNetCore.Mvc;
using WildPath.Data;
using WildPath.Repositories;
using WildPath.EfModels;

namespace WildPath.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly WildPathDbContext _wpdb;
        public TransactionController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration configuration, WildPathDbContext wpdb)
        {
            _logger = logger;
            this._context = db;
            _configuration = configuration;
            _wpdb = wpdb;
        }

        public IActionResult Index()
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_context);


            TransactionRepo transactionRepo = new TransactionRepo(_wpdb);




            return View(transactionRepo.GetAll());

        }
    }
}
