using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WildPath.Models;

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

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Return> Returns { get; set; }

    public virtual DbSet<ReturnItem> ReturnItems { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionItem> TransactionItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.PkAddressId).HasName("PK__Address__708062569AC85D43");

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

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.PkItemId).HasName("PK__Items__368A666D67A1CFA1");

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

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.PkEmail).HasName("PK__Profile__539A2769DBE818A5");

            entity.ToTable("Profile");

            entity.Property(e => e.PkEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pkEmail");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.FkMailingAddressId).HasColumnName("fkMailingAddressId");
            entity.Property(e => e.FkShippingAddressId).HasColumnName("fkShippingAddressId");
            entity.Property(e => e.IsAdmin)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("isAdmin");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.Phone).HasColumnName("phone");

            entity.HasOne(d => d.FkMailingAddress).WithMany(p => p.ProfileFkMailingAddresses)
                .HasForeignKey(d => d.FkMailingAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Profile__fkMaili__382F5661");

            entity.HasOne(d => d.FkShippingAddress).WithMany(p => p.ProfileFkShippingAddresses)
                .HasForeignKey(d => d.FkShippingAddressId)
                .HasConstraintName("FK__Profile__fkShipp__39237A9A");
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
                .HasConstraintName("FK__Refunds__fkRetur__4589517F");
        });

        modelBuilder.Entity<Return>(entity =>
        {
            entity.HasKey(e => e.PkReturnId).HasName("PK__Returns__66E126CB09C7D9D8");

            entity.Property(e => e.PkReturnId).HasColumnName("pkReturnId");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
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
                .HasConstraintName("FK__Returns__fkTrans__43A1090D");
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
                .HasConstraintName("FK__ReturnIte__fkIte__4865BE2A");

            entity.HasOne(d => d.FkReturn).WithMany()
                .HasForeignKey(d => d.FkReturnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReturnIte__fkRet__477199F1");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.PkTransactionId).HasName("PK__Transact__D33544062E01D440");

            entity.Property(e => e.PkTransactionId).HasColumnName("pkTransactionId");
            entity.Property(e => e.FkEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("fkEmail");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("date")
                .HasColumnName("purchaseDate");
            entity.Property(e => e.ShippingMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("shippingMethod");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.FkEmailNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FkEmail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__fkEma__3BFFE745");
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
                .HasConstraintName("FK__Transacti__fkIte__40C49C62");

            entity.HasOne(d => d.FkTransaction).WithMany()
                .HasForeignKey(d => d.FkTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__fkTra__3FD07829");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
