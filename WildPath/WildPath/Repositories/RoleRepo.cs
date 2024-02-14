using WildPath.Data;
using WildPath.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WildPath.Data;

namespace WildPath.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _db;

        public RoleRepo(ApplicationDbContext context)
        {
            this._db = context;
            CreateInitialRole();
        }

        public IEnumerable<RoleVM> GetAllRoles()
        {
            var roles = _db.Roles.Select(r => new RoleVM
            {
                Id = r.Id,
                RoleName = r.Name
            });

            return roles;
        }

        //public RoleVM GetRole(string id)
        //{


        //    var role = _db.Roles.Where(r => r.Id == id)
        //                        .FirstOrDefault();

        //    if (role != null)
        //    {
        //        return new RoleVM() { RoleName = role.Name, Id = role.Id };
        //    }
        //    return null;
        //}
        public RoleVM GetRole(string roleName)
        {
            var role = _db.Roles.Where(r => r.Name == roleName)
                                .FirstOrDefault();

            if (role != null)
            {
                return new RoleVM() { RoleName = role.Name };
            }
            return null;
        }

        public bool CreateRole(string roleId, string roleName)
        {
            bool isSuccess = true;

            try
            {
                if (_db.Roles.Any(r => r.Id == roleId))
                {
                    isSuccess = false;
                    return isSuccess;
                }

                _db.Roles.Add(new IdentityRole
                {
                    Id = roleId,
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public SelectList GetRoleSelectList()
        {
            var roles = GetAllRoles().Select(r => new
            SelectListItem
            {
                Value = r.RoleName,
                Text = r.RoleName
            });

            var roleSelectList = new SelectList(roles,
                                               "Value",
                                               "Text");
            return roleSelectList;
        }
       
        public void CreateInitialRole()
        {
            const string ADMIN = "Admin";
            const string ADMIN_ID = "Ad";

            var role = GetRole(ADMIN);

            if (role == null)
            {
                CreateRole(ADMIN_ID, ADMIN);
            }
        }

        public bool DeleteRole(string roleName)
        {
            var role = _db.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                _db.Roles.Remove(role);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

    }

}
