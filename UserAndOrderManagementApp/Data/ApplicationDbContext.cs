using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UserAndOrderManagement.Models;

namespace UserAndOrderManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.Id);

           modelBuilder.Entity<Order>()
                .Property(o => o.UserName)
                .HasColumnName("UserName");


            modelBuilder.Entity<Order>()
                .Property(o => o.ProductName)
                .HasColumnName("ProductName");

            modelBuilder.Entity<Order>()
                .Property(o => o.Price)
                .HasColumnName("Price");
        }
    }




}
