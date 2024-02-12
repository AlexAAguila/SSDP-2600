using WildPath.Data;
using WildPath.Repositories;
using WildPath.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace WildPath.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyRegisteredUserRepo _myRegisteredUserRepo;
        private readonly UserRoleRepo _userRoleRepo;

        public UserRoleController(ApplicationDbContext context
                                 , UserManager<IdentityUser> userManager
                                 , RoleManager<IdentityRole> roleManager
                                 , MyRegisteredUserRepo myRegisteredUserRepo
                                 , UserRoleRepo userRoleRepo)
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _myRegisteredUserRepo = myRegisteredUserRepo;
            _userRoleRepo = userRoleRepo;
        }

        public ActionResult Index(string message)
        {
            UserRepo userRepo = new UserRepo(_db);
            IEnumerable<UserVM> users = userRepo.GetAllUsers();
            ViewBag.Message = message;

            return View(users);
        }

        public async Task<IActionResult> Detail(string userName,
                                                string message = "")
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);


            var roles = await userRoleRepo.GetUserRolesAsync(userName);
            var user = _myRegisteredUserRepo.GetUserByName(userName);

            ViewBag.Name = user != null ? user.FirstName + " " + user.LastName : userName;
            ViewBag.Message = message;
            ViewBag.UserName = userName;

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
        public async Task<ActionResult> Delete(string userName, string roleName)
        {
            UserRoleVM userRoleVM = new UserRoleVM
            {
                Email = userName,
                RoleName = roleName
            };
            return View(userRoleVM);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(UserRoleVM userRoleVM)
        {
            bool isSuccess = await _userRoleRepo.RemoveUserRoleAsync(userRoleVM);

            if (isSuccess)
            {
                return RedirectToAction("Detail", new { userName = userRoleVM.Email });
            }
            else
            {
                ModelState.AddModelError("", "Role not removed.");
            }
            return View(userRoleVM);
        }

    }
}
