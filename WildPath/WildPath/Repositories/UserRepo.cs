using WildPath.Data;
using WildPath.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using WildPath.Data;

namespace WildPath.Repositories
{
    public class UserRepo
    {
        private readonly ApplicationDbContext _db;

        public UserRepo(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IEnumerable<UserVM> GetAllUsers()
        {
            IEnumerable<UserVM> users =
            _db.Users.Select(u => new UserVM { Email = u.Email });

            return users;
        }

        public SelectList GetUserSelectList()
        {
            IEnumerable<SelectListItem> users =
                GetAllUsers().Select(u => new SelectListItem
                {
                    Value = u.Email,
                    Text = u.Email
                });

            SelectList roleSelectList = new SelectList(users,
                                                      "Value",
                                                      "Text");
            return roleSelectList;
        }

    }
}
