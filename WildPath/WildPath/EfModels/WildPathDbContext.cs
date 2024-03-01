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

    public virtual DbSet<ImageStore> ImageStores { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<MyRegisteredUser> MyRegisteredUsers { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=tcp:wildpathserver.database.windows.net,1433;Initial Catalog=WildPathDb;Persist Security Info=False;User ID=WildPathAdmin;Password=P@ssw0rd!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.PkAddressId).HasName("PK__Address__7080625608E7B415");

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

        modelBuilder.Entity<ImageStore>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.Property(e => e.FileName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.PkItemId).HasName("PK__Items__368A666DC18DCB59");

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
            entity.Property(e => e.ItemImageId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("itemImageId");
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

        modelBuilder.Entity<MyRegisteredUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MyRegist__3214EC270C4A87ED");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FkMailingAdressId).HasColumnName("fkMailingAdressId");
            entity.Property(e => e.FkShippingAdressId).HasColumnName("fkShippingAdressId");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone).HasColumnName("phone");

            entity.HasOne(d => d.FkMailingAdress).WithMany(p => p.MyRegisteredUserFkMailingAdresses)
                .HasForeignKey(d => d.FkMailingAdressId)
                .HasConstraintName("FK__MyRegiste__fkMai__36470DEF");

            entity.HasOne(d => d.FkShippingAdress).WithMany(p => p.MyRegisteredUserFkShippingAdresses)
                .HasForeignKey(d => d.FkShippingAdressId)
                .HasConstraintName("FK__MyRegiste__fkShi__373B3228");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Transact__A0D9EFC68C8F4A22");

            entity.Property(e => e.PaymentId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("paymentId");
            entity.Property(e => e.Amount)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("amount");
            entity.Property(e => e.CreateTime)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("createTime");
            entity.Property(e => e.Currency)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.PayerEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("payerEmail");
            entity.Property(e => e.PayerName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("payerName");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("paymentMethod");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
