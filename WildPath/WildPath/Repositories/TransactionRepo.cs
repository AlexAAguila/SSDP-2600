using WildPath.EfModels;
using WildPath.Models;
using WildPath.ViewModels;
using System;
using System.Collections.Generic;
using WildPath.Data;

namespace WildPath.Repositories
{
    public class TransactionRepo
    {

        private readonly WildPathDbContext _context;

        public TransactionRepo(WildPathDbContext context)
        {
            this._context = context;
        }


        public IEnumerable<Transaction> GetAll()
        {
            var s = _context;
            return _context.Transactions;
        }


        public string Add(PayPalConfirmationModel payPalConfirmationModel)
        {
            var address = new Address()
            {
                Address1 = payPalConfirmationModel.Address.Address1,
                City = payPalConfirmationModel.Address.City,
                Province = payPalConfirmationModel.Address.Province,
                PostalCode = payPalConfirmationModel.Address.PostalCode,
                Unit = payPalConfirmationModel.Address.Unit
            };
            _context.Addresses.Add(address);
            _context.SaveChanges();
            //address = _context.Addresses.Find(address.PkAddressId);
            var transaction = new Transaction()
            {
                PaymentId = payPalConfirmationModel.TransactionId,
                CreateTime = DateTime.Now.ToString("dd-MM-yyyy, HH:mm"),
                PayerName = payPalConfirmationModel.PayerName,
                PayerEmail = payPalConfirmationModel.PayerEmail,
                Amount = payPalConfirmationModel.Amount,
                Currency = "CAD",
                PaymentMethod = payPalConfirmationModel.PaymentMethod,
                ShippingMethod = payPalConfirmationModel.ShippingMethod,
                FkAddressId = address.PkAddressId,
                
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return "Transaction added successfully";
        }

        public IEnumerable<Address> GetAddress(string id)
        {
            var registeredUserId = _context.MyRegisteredUsers.FirstOrDefault(p => p.Email == id); 

            var address = _context.Addresses.Where(a => a.PkAddressId == registeredUserId.FkShippingAdressId);

            return address.ToList();
        }


    }
}
