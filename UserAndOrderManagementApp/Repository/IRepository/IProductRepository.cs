using UserAndOrderManagement.Models;

namespace UserAndOrderManagement.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product entity);
    }
}
