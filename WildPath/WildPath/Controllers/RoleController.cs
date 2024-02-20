using WildPath.Data;
using WildPath.Repositories;
using WildPath.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WildPath.Data;
using Microsoft.AspNetCore.Authorization;

namespace WildPath.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }

        //public ActionResult Index()
        //{
        //    RoleRepo roleRepo = new RoleRepo(_db);
        //    return View(roleRepo.GetAllRoles());
        //}

        public ActionResult Index(string message = "")
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.Message = message;
            return View(roleRepo.GetAllRoles());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                RoleRepo roleRepo = new RoleRepo(_db);
                bool isSuccess =
                    roleRepo.CreateRole(roleVM.Id, roleVM.RoleName);

                if (isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState
                    .AddModelError("", "Role creation failed.");
                    ModelState
                    .AddModelError("", "The role may already" +
                                       " exist.");
                }
            }

            return View(roleVM);
        }


        [HttpGet]
        public ActionResult Delete(string roleName)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            var role = roleRepo.GetRole(roleName);
            if (role == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(role);

        }

        [HttpPost]
        public ActionResult Delete(RoleVM roleVM)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            bool isSuccess = roleRepo.DeleteRole(roleVM.RoleName);

            if (isSuccess)
            {
                string message = "Role deletion success.";
                return RedirectToAction(nameof(Index), new { message = message });
            }
            else
            {
                ModelState.AddModelError("", "Role deletion failed.");
            }
            return View(roleVM);
        }

        public IActionResult Details(string id)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            RoleVM role = roleRepo.GetRole(id);
            return View(role);
        }


    }
}
