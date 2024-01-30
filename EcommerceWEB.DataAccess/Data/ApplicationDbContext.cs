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
            new Category { Id = Guid.NewGuid(), CatergoryName = "Action", DisplayOrder = 1 },
            new Category { Id = Guid.NewGuid(), CatergoryName = "SciFi", DisplayOrder = 2 },
            new Category { Id = Guid.NewGuid(), CatergoryName = "History", DisplayOrder = 3 }

            );
    }
}
