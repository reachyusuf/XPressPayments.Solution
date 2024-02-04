using System.Linq.Expressions;

namespace XPressPayments.Data.EFRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(Guid id);
        Task<T> Get(int id);

        Task<T> Get(long id);

        Task<T> Get(string id);
        IQueryable<T> Query();

        Task<T> Get(Expression<Func<T, bool>> match, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<T> Get();
        Task<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, IEnumerable<string> includePaths = null);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeProperties = "", int? page = null, int? pageSize = null);
        IQueryable<T> GetIQueryable(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> GetAsNoTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                       int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAsNoTracking(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties);


        Task<T> Add(T entity);
        Task<T> AddAndSave(T entity);
        Task<IList<T>> AddRange(IList<T> entity);

        Task<T> Edit(T updated);

        Task<IList<T>> EditRange(IList<T> entity);
        Task Remove(T t);
        Task RemoveRange(IEnumerable<T> entities);

        Task<int> CountWithFilter(Expression<Func<T, bool>> filter = null);
        Task<decimal> SumWithFilter(Expression<Func<T, decimal>> column, Expression<Func<T, bool>> filter = null);

        bool Exist(Expression<Func<T, bool>> predicate);

        IQueryable<T> PaginatedListAsNoTracking(IQueryable<T> source, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties);
    }

}
