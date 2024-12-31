namespace OnlineShop.Infrastructure.Data.Repository.Interfaces
{
    using System.Linq.Expressions;

    public interface IRepository<TType> where TType : class
    {
        IEnumerable<TType> GetAll();
        IQueryable<TType> GetAllAttached();
		Task<List<TType>> GetAllAsync();
		IQueryable<TType> GetAllPagedAttached(int pageIndex, int pageSize);
		IQueryable<TType> GetAllSearchedPagedAttached(Expression<Func<TType, bool>> searchExpression, int pageIndex, int pageSize);
		Task<int> AllCountAsync();
		Task<int> AllSearchedCountAsync(Expression<Func<TType, bool>> searchExpression);
		TType GetById(Guid id);
        Task<TType> GetByIdAsync(Guid id);
        bool Remove(Guid id);
        void Add(in TType sender);
        void Update(in TType sender);
        int Save();
        Task<int> SaveAsync();
        public TType Select(Expression<Func<TType, bool>> predicate);
        public Task<TType> SelectAsync(Expression<Func<TType, bool>> predicate);
    }
}