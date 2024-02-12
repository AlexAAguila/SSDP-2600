using WildPath.Data;
using WildPath.Repositories;
using WildPath.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WildPath.Data;

namespace WildPath.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            RoleRepo roleRepo = new RoleRepo(_db);
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
        public ActionResult Delete(string id)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            RoleVM role = roleRepo.GetRole(id);
            return View(role);
        }

        [HttpPost]
        public ActionResult Delete(RoleVM roleVM)
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            bool isSuccess = roleRepo.DeleteRole(roleVM);
            if (isSuccess)
            {
                return RedirectToAction(nameof(Index), new { message = "Role deleted successfully." });
            }
            else
            {
                ModelState.AddModelError("", "Role deletion failed.");
                ModelState.AddModelError("", "The role may be assigned to a user.");
            }
            return View(roleVM);
        }

    }
}
