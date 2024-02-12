using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WildPath.Models;
using WildPath.ViewModels;

namespace WildPath.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MyRegisteredUser> MyRegisteredUsers { get; set; }
        public DbSet<WildPath.ViewModels.RoleVM> RoleVM { get; set; } = default!;
        public DbSet<WildPath.ViewModels.UserRoleVM> UserRoleVM { get; set; } = default!;
        public DbSet<WildPath.ViewModels.UserVM> UserVM { get; set; } = default!;

    }
}
