using WildPath.Data;
using WildPath.Repositories;
using WildPath.ViewModels;
using WildPath.EfModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace WildPath.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly WildPathDbContext _wpdb;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyRegisteredUserRepo _myRegisteredUserRepo;
        private readonly UserRoleRepo _userRoleRepo;

        public UserRoleController(ApplicationDbContext context
                                 , UserManager<IdentityUser> userManager
                                 , RoleManager<IdentityRole> roleManager
                                 , MyRegisteredUserRepo myRegisteredUserRepo
                                 , UserRoleRepo userRoleRepo
                                 , WildPathDbContext wpdb)
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _myRegisteredUserRepo = myRegisteredUserRepo;
            _userRoleRepo = userRoleRepo;
            _wpdb = wpdb;   
        }

        public ActionResult Index(string message)
        {
            UserRepo userRepo = new UserRepo(_db);
            IEnumerable<UserVM> users = userRepo.GetAllUsers();
            ViewBag.Message = message;

            return View(users);
        }

        public async Task<IActionResult> Detail(string userName, string message = "")
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            MyRegisteredUserRepo myRegisteredUserRepo = new MyRegisteredUserRepo(_wpdb);

            var roles = await userRoleRepo.GetUserRolesAsync(userName);
            var user = await _userManager.FindByNameAsync(userName);
            var userEmail = user != null ? user.Email : "";

            ViewBag.Message = message;
            ViewBag.UserName = userEmail;

            return View(roles);
        }


        public ActionResult Create()
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();


            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                try
                {
                    var addUR =
                    await userRoleRepo.AddUserRoleAsync(userRoleVM.Email,
                                                        userRoleVM.RoleName);

                    string message = $"{userRoleVM.RoleName} permissions" +
                                     $" successfully added to " +
                                     $"{userRoleVM.Email}.";

                    return RedirectToAction("Detail", "UserRole",
                                      new
                                      {
                                          userName = userRoleVM.Email,
                                          message = message
                                      });
                }
                catch
                {
                    ModelState.AddModelError("", "UserRole creation failed.");
                    ModelState.AddModelError("", "The Role may exist " +
                                                 "for this user.");
                }
            }

            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();

            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string email, string role)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(); 
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(role))
            {
                return NotFound();
            }

            var model = new UserRoleVM
            {
                Email = email,
                RoleName = role
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(UserRoleVM model)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                var removed = await userRoleRepo.RemoveUserRoleAsync(model.Email, model.RoleName);
                if (removed)
                {
                    ViewBag.Message = $"Role '{model.RoleName}' has been removed from user '{model.Email}'.";

                    return RedirectToAction("Detail", new { userName = model.Email, message = ViewBag.Message });
                }
            }
            return View(model);
        }
    }
}