using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WildPath.EfModels;

public partial class WildPathDbContext : DbContext
{
    public WildPathDbContext()
    {
    }

    public WildPathDbContext(DbContextOptions<WildPathDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<ContactInfo> ContactInfos { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Return> Returns { get; set; }

    public virtual DbSet<ReturnItem> ReturnItems { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionItem> TransactionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.PkAddressId).HasName("PK__Address__708062568145BD1C");

            entity.ToTable("Address");

            entity.Property(e => e.PkAddressId).HasColumnName("pkAddressId");
            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("postalCode");
            entity.Property(e => e.Province)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("province");
            entity.Property(e => e.Unit).HasColumnName("unit");
        });

        modelBuilder.Entity<ContactInfo>(entity =>
        {
            entity.HasKey(e => e.PkShippingProfile).HasName("PK__ContactI__335917A3C9BCBB84");

            entity.ToTable("ContactInfo");

            entity.Property(e => e.PkShippingProfile).HasColumnName("pkShippingProfile");
            entity.Property(e => e.FkMailingAddressId).HasColumnName("fkMailingAddressId");
            entity.Property(e => e.FkShippingAddressId).HasColumnName("fkShippingAddressId");
            entity.Property(e => e.Phone).HasColumnName("phone");

            entity.HasOne(d => d.FkMailingAddress).WithMany(p => p.ContactInfoFkMailingAddresses)
                .HasForeignKey(d => d.FkMailingAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ContactIn__fkMai__6383C8BA");

            entity.HasOne(d => d.FkShippingAddress).WithMany(p => p.ContactInfoFkShippingAddresses)
                .HasForeignKey(d => d.FkShippingAddressId)
                .HasConstraintName("FK__ContactIn__fkShi__6477ECF3");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.PkItemId).HasName("PK__Items__368A666D4B88D4FE");

            entity.Property(e => e.PkItemId).HasColumnName("pkItemId");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.Colour)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("colour");
            entity.Property(e => e.ItemDetails)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("itemDetails");
            entity.Property(e => e.ItemName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("itemName");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.Supplier)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("supplier");
            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.FkReturnId).HasColumnName("fkReturnId");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.FkReturn).WithMany()
                .HasForeignKey(d => d.FkReturnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Refunds__fkRetur__6FE99F9F");
        });

        modelBuilder.Entity<Return>(entity =>
        {
            entity.HasKey(e => e.PkReturnId).HasName("PK__Returns__66E126CB86F18E3A");

            entity.Property(e => e.PkReturnId).HasColumnName("pkReturnId");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.FkTransactionId).HasColumnName("fkTransactionId");
            entity.Property(e => e.ReasonForReturn)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("reasonForReturn");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.FkTransaction).WithMany(p => p.Returns)
                .HasForeignKey(d => d.FkTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Returns__fkTrans__6E01572D");
        });

        modelBuilder.Entity<ReturnItem>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FkItemId).HasColumnName("fkItemId");
            entity.Property(e => e.FkReturnId).HasColumnName("fkReturnId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.FkItem).WithMany()
                .HasForeignKey(d => d.FkItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReturnIte__fkIte__72C60C4A");

            entity.HasOne(d => d.FkReturn).WithMany()
                .HasForeignKey(d => d.FkReturnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReturnIte__fkRet__71D1E811");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.PkTransactionId).HasName("PK__Transact__D33544068C1A6BD1");

            entity.Property(e => e.PkTransactionId).HasColumnName("pkTransactionId");
            entity.Property(e => e.PurchaseDate).HasColumnName("purchaseDate");
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("shippingMethod");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<TransactionItem>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FkItemId).HasColumnName("fkItemId");
            entity.Property(e => e.FkTransactionId).HasColumnName("fkTransactionId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.FkItem).WithMany()
                .HasForeignKey(d => d.FkItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__fkIte__6B24EA82");

            entity.HasOne(d => d.FkTransaction).WithMany()
                .HasForeignKey(d => d.FkTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__fkTra__6A30C649");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
