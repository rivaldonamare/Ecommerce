using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWEB.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Update(Product product)
    {
        var objFromDB = _context.Products.FirstOrDefault(x => x.Id == product.Id);

        if (objFromDB != null)
        {
            objFromDB.Title = product.Title;
            objFromDB.Description = product.Description;
            objFromDB.ISBN = product.ISBN;
            objFromDB.Price = product.Price;
            objFromDB.ListPrice = product.ListPrice;
            objFromDB.Price100 = product.Price100;
            objFromDB.Price50 = product.Price50;
            objFromDB.Category = product.Category;
            objFromDB.CategoryId = product.CategoryId;
            objFromDB.Author = product.Author;
            if (objFromDB.ImageUrl != null)
            {
                objFromDB.ImageUrl = product.ImageUrl;
            }

        }
    }
}
