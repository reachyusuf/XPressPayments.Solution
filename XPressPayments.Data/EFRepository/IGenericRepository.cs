using System.Linq.Expressions;

namespace XPressPayments.Data.EFRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> AddAndSave(T entity);
        Task<IList<T>> AddRange(IList<T> entity);
        Task<T> Edit(T updated);
        Task<IList<T>> EditRange(IList<T> entity);
        Task Remove(T t);
        Task RemoveRange(IEnumerable<T> entities);
        bool Exist(Expression<Func<T, bool>> predicate);
        Task<bool> Exist();
        Task<T> Find(long id);
        Task<T> Find(Guid id);
        Task<T> Find(string id);
        Task<T> Find(int id);
        Task<T> Find(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<T> Find(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, IEnumerable<string> includePaths);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? page = null, int? pageSize = null, string includeProperties = "");
        IQueryable<T> Query();
        Task<int> CountWithFilter(Expression<Func<T, bool>> filter = null);
        Task<decimal> SumWithFilter(Expression<Func<T, decimal>> column, Expression<Func<T, bool>> filter = null);
        Task<int> SaveChangesAsync();
    }
}
