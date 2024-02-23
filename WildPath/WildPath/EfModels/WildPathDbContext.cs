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

    public virtual DbSet<ImageStore> ImageStores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:wildpathserver.database.windows.net,1433;Initial Catalog=WildPathDb;Persist Security Info=False;User ID=WildPathAdmin;Password=P@ssw0rd!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImageStore>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.Property(e => e.FileName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
