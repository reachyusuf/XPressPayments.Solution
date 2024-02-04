using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XPressPayments.Data.Entities;

namespace XPressPayments.Data.DataAccess
{
    public class AppDbContext : IdentityDbContext<UserInfo, Role, string>
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<UserInfo> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //--UsersInfo Fluent API configurations
            builder.Entity<UserInfo>().HasKey(table => new { table.Id });
            builder.Entity<UserInfo>().Property(a => a.Id).HasMaxLength(50);
            builder.Entity<UserInfo>().Property(a => a.Role).HasMaxLength(20);
            builder.Entity<UserInfo>().Property(a => a.ProfileName).HasMaxLength(100);
            builder.Entity<UserInfo>().Property(a => a.UserName).HasMaxLength(50);
            builder.Entity<UserInfo>().Property(a => a.Email).HasMaxLength(100);
        }
    }
}
