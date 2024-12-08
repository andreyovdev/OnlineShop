namespace OnlineShop.Infrastructure.Data.Repository
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Infrastructure.Data.Repository.Interfaces;

    public class BaseRepository<TType> : IRepository<TType> 
        where TType : class
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<TType> dbSet;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TType>();
        }

        public IEnumerable<TType> GetAll()
        {
            return this.dbSet.ToList();
        }

        public IQueryable<TType> GetAllAttached()
        {
            return this.dbSet.AsQueryable();
        }

        public Task<List<TType>> GetAllAsync()
        {
            return this.dbSet.ToListAsync();
        }

        public TType GetById(int id)
        {
            return this.dbSet.Find(id);
        }

        public async Task<TType> GetByIdAsync(int id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public bool Remove(int id)
        {
            var product = this.dbSet.Find(id);
            if (product is { })
            {
                this.dbSet.Remove(product);
                return true;
            }

            return false;
        }

        public void Add(in TType sender)
        {
            dbContext.Add(sender).State = EntityState.Added;
        }

        public void Update(in TType sender)
        {
            dbContext.Entry(sender).State = EntityState.Modified;
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public TType Select(
            Expression<Func<TType, bool>> predicate)
        {
            return this.dbSet.FirstOrDefault(predicate);
        }

        public async Task<TType> SelectAsync(
            Expression<Func<TType, bool>> predicate)
        {
            return await this.dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
