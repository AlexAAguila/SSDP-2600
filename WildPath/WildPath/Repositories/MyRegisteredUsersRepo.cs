using WildPath.Data;
using WildPath.EfModels;
using WildPath.ViewModels;

namespace WildPath.Repositories
{
    public class MyRegisteredUserRepo
    {
        private readonly WildPathDbContext _db;
        private readonly ApplicationDbContext _applicationDbContext;

        public MyRegisteredUserRepo(WildPathDbContext context)
        {
            _db = context;
        }

        public MyRegisteredUser GetUserByName(string userName)
        {
            return _db.MyRegisteredUsers.FirstOrDefault(u => u.Email == userName);
        }

        public string GetFirstAndLastNameByEmail(string email)
        {
            return _db.MyRegisteredUsers
                    .Where(u => u.Email == email)
                    .Select(u => u.FirstName + " " + u.LastName).FirstOrDefault(); ;
        }
        
        public string AddFkAddress(CheckoutVM vm, string accountEmail)
        {
            var user = GetUserByName(accountEmail);
            user.FkShippingAdress = vm.Address;

            _db.MyRegisteredUsers.Update(user);
            _db.SaveChanges();
                    
            return "Address added";
        }   
        //public string GetUserIdByEmail(string email)
        //{
        //    return _applicationDbContext.Users
        //                            .Where(u => u.UserName == email)
        //                            .Select(u => u.Id)
        //                            .FirstOrDefault(); ;
        //}

        public void Add(MyRegisteredUser user)
        {
            _db.MyRegisteredUsers.Add(user);
            _db.SaveChanges();
        }
    }
}
