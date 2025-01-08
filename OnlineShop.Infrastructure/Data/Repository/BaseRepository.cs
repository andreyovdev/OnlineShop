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

		public IQueryable<TType> GetFilteredChunkAsync(
            Expression<Func<TType, bool>> predicate, 
            List<(Expression<Func<TType, object>> orderBy, bool isDescending)> orderByList, 
            int skip, 
            int take)
		{
			int totalEntities = this.dbSet.Count(predicate);

			take = Math.Max(0, Math.Min(take, totalEntities - skip));

			var query = this.dbSet.Where(predicate);

			foreach (var orderByItem in orderByList)
			{
				query = orderByItem.isDescending
					? query.OrderByDescending(orderByItem.orderBy)
					: query.OrderBy(orderByItem.orderBy);
			}

			return query
				.Skip(skip)
				.Take(take)
				.AsQueryable();
		}

		public async Task<int> AllCountAsync()
		{
			return await this.dbSet.CountAsync();
		}

		public async Task<int> AllFilteredCountAsync(Expression<Func<TType, bool>> predicate)
		{
			return await this.dbSet
					.Where(predicate)
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

        public bool RemoveById(Guid id)
        {
            var product = this.dbSet.Find(id);
            if (product is { })
            {
                this.dbSet.Remove(product);
                return true;
            }

            return false;
        }

		public bool RemoveByEntity(TType entity)
		{
			if (entity != null)
			{
				this.dbSet.Remove(entity);
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
