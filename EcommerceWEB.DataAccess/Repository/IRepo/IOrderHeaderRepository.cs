using EcommerceWEB.Models.Models;

namespace EcommerceWEB.DataAccess.Repository.IRepo;

public interface IOrderHeaderRepository : IRepositroy<OrderHeader>
{
    void Update(OrderHeader obj);
    void UpdateStatus(Guid id, string orderStatus, string paymentStatus);
    void UpdateStripePaymentId(Guid id, string sessionId, string paymentIntentId);
}
