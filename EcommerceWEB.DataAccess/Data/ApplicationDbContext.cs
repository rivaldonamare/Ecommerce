using EcommerceWEB.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWEB.DataAccess.Data;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = Guid.NewGuid(), CategoryName = "Action", DisplayOrder = 1 },
            new Category { Id = Guid.NewGuid(), CategoryName = "SciFi", DisplayOrder = 2 },
            new Category { Id = Guid.NewGuid(), CategoryName = "History", DisplayOrder = 3 }
            );
    }
}
