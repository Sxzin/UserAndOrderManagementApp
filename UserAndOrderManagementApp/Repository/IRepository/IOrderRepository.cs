using System.Linq.Expressions;

namespace UserAndOrderManagementApp.Repository.IRepository
{
    public interface IOrderRepository<T> where T : class
    {
        Task<List<T>> GetOrdersAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateOrderAsync(T entity);
        Task DeleteOrderAsync(T entity);
        Task SaveAsync();

    }
}
