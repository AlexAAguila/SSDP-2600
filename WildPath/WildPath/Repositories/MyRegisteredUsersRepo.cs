using WildPath.Data;
using WildPath.EfModels;

namespace WildPath.Repositories
{
    public class MyRegisteredUserRepo
    {
        private readonly WildPathDbContext _db;

        public MyRegisteredUserRepo(WildPathDbContext context)
        {
            _db = context;
        }

        public MyRegisteredUser GetUserByName(string userName)
        {
            return _db.MyRegisteredUsers.FirstOrDefault(u => u.Email == userName);
        }

        public void Add(MyRegisteredUser user)
        {
            _db.MyRegisteredUsers.Add(user);
            _db.SaveChanges();
        }
    }
}
