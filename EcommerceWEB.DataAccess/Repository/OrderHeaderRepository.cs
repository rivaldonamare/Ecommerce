using EcommerceWEB.DataAccess.Data;
using EcommerceWEB.DataAccess.Repository.IRepo;
using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository;

public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
{

    private readonly ApplicationDbContext _context;

    public OrderHeaderRepository(ApplicationDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public void Update(OrderHeader obj)
    {
        _context.OrderHeaders.Update(obj);
    }
}
