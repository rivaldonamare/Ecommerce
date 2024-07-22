using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    private readonly ApplicationDbContext _context;

    public OrderDetailsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public void Update(OrderDetails obj)
    {
        _context.OrderDetails.Update(obj);
    }
}
