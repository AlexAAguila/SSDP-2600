﻿using WildPath.EfModels;
using WildPath.Models;
using WildPath.ViewModels;
using System;
using System.Collections.Generic;
using WildPath.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WildPath.Repositories
{
    public class TransactionRepo
    {

        private readonly WildPathDbContext _context;
        private readonly ApplicationDbContext _applicationDbContext;

        public TransactionRepo(WildPathDbContext context)
        {
            this._context = context;
        }


        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions.Include(t => t.FkAddress);
        }

        public IEnumerable<Transaction> GetTransactionsByUserId(string userId)
        {
            return _context.Transactions.Where(t => t.FkUserId == userId).ToList();
        }
        public IEnumerable<Transaction> GetTransactionsByTransactionId(string PayerId)
        {
            return _context.Transactions.Where(t => t.PaymentId == PayerId);
        }

        public string Add(CheckoutVM CheckoutVM, string userName, string userEmail)
        {
            var address = new Address()
            {
                Address1 = CheckoutVM.Address.Address1,
                City = CheckoutVM.Address.City,
                Province = CheckoutVM.Address.Province,
                PostalCode = CheckoutVM.Address.PostalCode,
                Unit = CheckoutVM.Address.Unit,
                
            };
            _context.Addresses.Add(address);
            _context.SaveChanges();
            var transaction = new Transaction()
            {
                PaymentId = CheckoutVM.TransactionId,
                CreateTime = DateTime.Now.ToString("dd-MM-yyyy, HH:mm"),
                PayerName = CheckoutVM.PayerFullName,
                PayerEmail = userEmail,
                Amount = CheckoutVM.GrandTotal.ToString(),
                FkAddress = address,
                Currency = CheckoutVM.CurrencyCode,
                PaymentMethod = "PayPal",
                ShippingMethod = CheckoutVM.ShippingMethod,
                FkAddressId = address.PkAddressId,
                FkUserId = userName

            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return "Transaction added successfully";
        }


        public IEnumerable<Transaction> GetAllTransactions()
        {
            return _context.Transactions;
        }
        public IEnumerable<Address> GetAddress(string id)
        {
            var registeredUserId = _context.MyRegisteredUsers.FirstOrDefault(p => p.Email == id);

            List<Address> addresses = new List<Address>();

            if (registeredUserId != null)
            {
                Address address = _context.Addresses.FirstOrDefault(a => a.PkAddressId == registeredUserId.FkShippingAdressId);
                if (address != null)
                {
                    addresses.Add(address);
                }
            }

            return addresses;
        }

        public ShippingInfo GetShippingInfo()
        {
            var shippingInfo = _context.ShippingInfos.FirstOrDefault();
            return shippingInfo;
        }

    }
}
