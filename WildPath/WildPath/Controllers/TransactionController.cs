using Microsoft.AspNetCore.Mvc;
using WildPath.Data;
using WildPath.Repositories;

namespace WildPath.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TransactionController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            this._context = db;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_context);


            TransactionRepo transactionRepo = new TransactionRepo(_context);




            return View(transactionRepo.GetAll());

        }
    }
}
