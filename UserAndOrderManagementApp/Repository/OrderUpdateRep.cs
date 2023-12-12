using UserAndOrderManagement.Data;
using UserAndOrderManagement.Models;
using UserAndOrderManagement.Repository.IRepository;
using UserAndOrderManagement.Repository;
using UserAndOrderManagementApp.Repository.IRepository;

namespace UserAndOrderManagementApp.Repository
{
    public class OrderUpdateRep : OrderRepository<Order>, IOrderUpdateRep
    {
        private readonly ApplicationDbContext _db;
        public OrderUpdateRep(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Order> UpdateAsync(Order entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.Orders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
