namespace OnlineShop.Infrastructure.Data.Repository.Interfaces
{
    using System.Linq.Expressions;

    public interface IRepository<TType> where TType : class
    {
        IEnumerable<TType> GetAll();
        IQueryable<TType> GetAllAttached();
		Task<List<TType>> GetAllAsync();
		IQueryable<TType> GetFilteredChunkAsync(Expression<Func<TType, bool>> predicate, List<(Expression<Func<TType, object>> orderBy, bool isDescending)> orderByList, int chunkIndex, int chunkSize);
		Task<int> AllCountAsync();
		Task<int> AllFilteredCountAsync(Expression<Func<TType, bool>> predicate);
		TType GetById(Guid id);
        Task<TType> GetByIdAsync(Guid id);
        bool RemoveById(Guid id);
        bool RemoveByEntity(TType entity);
        void Add(in TType sender);
        void Update(in TType sender);
        int Save();
        Task<int> SaveAsync();
        public TType Select(Expression<Func<TType, bool>> predicate);
        public Task<TType> SelectAsync(Expression<Func<TType, bool>> predicate);
    }
}