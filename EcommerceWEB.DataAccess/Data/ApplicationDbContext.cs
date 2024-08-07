﻿using EcommerceWEB.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWEB.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, CategoryName = "Action", DisplayOrder = 1 },
            new Category { Id = 2, CategoryName = "SciFi", DisplayOrder = 2 },
            new Category { Id = 3, CategoryName = "History", DisplayOrder = 3 }
            );

        modelBuilder.Entity<Product>().HasData(
           new Product
           {
               Id = Guid.NewGuid(),
               Title = "Fortune of Time",
               Author = "Billy Spark",
               Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
               ISBN = "SWD9999001",
               ListPrice = 99000,
               Price = 90000,
               Price50 = 85000,
               Price100 = 80000,
               CategoryId = 1,
               ImageUrl = "\\images\\product\\3075dc18-442e-40ab-94e9-6fa06e099c7d.jpg"
           },
            new Product
            {
                Id = Guid.NewGuid(),
                Title = "Dark Skies",
                Author = "Nancy Hoover",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "CAW777777701",
                ListPrice = 40000,
                Price = 30000,
                Price50 = 25000,
                Price100 = 20000,
                CategoryId = 2,
                ImageUrl = "\\images\\product\\a512016e-a47b-4e9f-9828-671e4ff9c2bb.jpg"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Title = "Vanish in the Sunset",
                Author = "Julian Button",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "RITO5555501",
                ListPrice = 55000,
                Price = 50000,
                Price50 = 40000,
                Price100 = 35000,
                CategoryId = 3,
                ImageUrl = "\\images\\product\\be8f6b36-ad4a-4603-b3c4-e854eb4ec3d1.jpg"
            });
    }
}
