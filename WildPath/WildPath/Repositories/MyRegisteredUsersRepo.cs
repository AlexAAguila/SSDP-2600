using Microsoft.EntityFrameworkCore;
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

        public MyRegisteredUser GetUserByEmail(string email)
        {
            return _db.MyRegisteredUsers.Include(u => u.FkMailingAdress).Include(u => u.FkShippingAdress).FirstOrDefault(u => u.Email == email);
        }

        public Address AddOrUpdateAddress(Address address)
        {
            if (address.PkAddressId == 0)
            {
                _db.Addresses.Add(address);
            }
            else
            {
                _db.Addresses.Update(address);
            }
            _db.SaveChanges();
            return address;
        }

        public void UpdateUser(MyRegisteredUser user)
        {
            if (_db.Entry(user).State == EntityState.Detached)
            {
                _db.MyRegisteredUsers.Attach(user);
            }

            _db.Entry(user).State = EntityState.Modified;

            _db.SaveChanges();
        }
    }
}
