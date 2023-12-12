using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserAndOrderManagement.Data;
using UserAndOrderManagement.Repository.IRepository;
using UserAndOrderManagementApp.Repository.IRepository;

namespace UserAndOrderManagementApp.Repository
{
    public class OrderRepository<T> : IOrderRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateOrderAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteOrderAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetOrdersAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
