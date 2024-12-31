namespace OnlineShop.Infrastructure.Data.Repository
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using Interfaces;

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

		public IQueryable<TType> GetAllPagedAttached(int pageIndex, int pageSize)
		{
			return this.dbSet.Skip((pageIndex - 1) * pageSize).Take(pageSize).AsQueryable();
		}

		public IQueryable<TType> GetAllSearchedPagedAttached(Expression<Func<TType, bool>> searchExpression, int pageIndex, int pageSize)
		{
			return this.dbSet
                .Where(searchExpression)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsQueryable();
		}

		public async Task<int> AllCountAsync()
		{
			return await this.dbSet.CountAsync();
		}


		public async Task<int> AllSearchedCountAsync(Expression<Func<TType, bool>> searchExpression)
		{
			return await this.dbSet
                .Where(searchExpression)
                .CountAsync();
		}

		public TType GetById(Guid id)
        {
            return this.dbSet.Find(id);
        }

        public async Task<TType> GetByIdAsync(Guid id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public bool Remove(Guid id)
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
