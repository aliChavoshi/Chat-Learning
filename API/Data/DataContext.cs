using System.Reflection;
using API.Entities;
using API.Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public  DbSet<Users> Users { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<UserLike> UserLike { get; set; }
        public DbSet<Message> Message { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}