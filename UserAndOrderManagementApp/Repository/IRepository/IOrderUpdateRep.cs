using UserAndOrderManagement.Models;

namespace UserAndOrderManagementApp.Repository.IRepository
{
    public interface IOrderUpdateRep : IOrderRepository<Order>
    {
        Task<Order> UpdateAsync(Order entity);
    }
}
