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
            var transaction = new Transaction()
            {
                PaymentId = payPalConfirmationModel.TransactionId,
                CreateTime = DateTime.Now.ToString("dd-MM-yyyy, HH:mm"),
                PayerName = payPalConfirmationModel.PayerName,
                PayerEmail = payPalConfirmationModel.PayerEmail,
                Amount = payPalConfirmationModel.Amount,
                Currency = "CAD",
                PaymentMethod = payPalConfirmationModel.PaymentMethod
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return "Transaction added successfully";
        }


    }
}
