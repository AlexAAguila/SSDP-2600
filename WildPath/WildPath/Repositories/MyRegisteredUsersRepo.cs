using Microsoft.EntityFrameworkCore;
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
