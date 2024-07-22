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

    public void UpdateStatus(Guid id, string orderStatus, string paymentStatus)
    {
       var order = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
        if(order != null)
        {
            order.OrderStatus = orderStatus;
            if(!string.IsNullOrEmpty(paymentStatus))
            {
                order.PaymentStatus = paymentStatus;
            }
        }
    }

    public void UpdateStripePaymentId(Guid id, string sessionId, string paymentIntentId)
    {
        var order = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
        if(!string.IsNullOrEmpty(sessionId))
        {
            order.SessionId = sessionId;
        }
        if(!string.IsNullOrEmpty(paymentIntentId))
        {
            order.PaymentIntentId = paymentIntentId;
            order.PaymentDate = DateTime.Now;
        }
    }
}
