using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XPressPayments.Data.DataAccess;
using XPressPayments.Data.UnitOfWork;

namespace XPressPayments.Data.EFRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly IUnitofWork _unitOfWork;
        private static readonly int _pageSize = 10;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _unitOfWork = new UnitofWork(context);
        }

        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> AddAndSave(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IList<T>> AddRange(IList<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            return entity;
        }

        public Task<T> Edit(T updated)
        {
            return Task.Run(() =>
            {
                if (updated is null)
                {
                    return null;
                }

                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
                return updated;
            });
        }

        public Task<IList<T>> EditRange(IList<T> entity)
        {
            return Task.Run(() =>
            {
                _context.Set<T>().UpdateRange(entity);
                return entity;
            });
        }

        public Task Remove(T t)
        {
            return Task.Run(() => _context.Set<T>().Remove(t));
        }

        public Task RemoveRange(IEnumerable<T> entities)
        {
            return Task.Run(() => _context.Set<T>().RemoveRange(entities));
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> exist = _context.Set<T>().Where(predicate);
            return exist.Any();
        }

        public async Task<bool> Exist()
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync();
            return result is not null;
        }

        public async Task<T> Find(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }  

        public async Task<T> Find(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, IEnumerable<string> includePaths)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null)
                query = query.Where(filter);
            if (orderBy is not null)
                query = orderBy(query);
            if (includePaths is not null)
            {
                query = includePaths.Aggregate(query, (current, includePath) => current.Include(includePath));
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    int? page = null, int? pageSize = null, string includeProperties = "")
        {
            pageSize = pageSize ?? _pageSize;
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            if (orderBy is not null)
            {
                query = orderBy(query);
            }
            if (string.IsNullOrEmpty(includeProperties) is false)
            {
                foreach (string includeProperty in includeProperties.Split
           (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (page is not null && pageSize is not null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return await query.ToListAsync();
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<int> CountWithFilter(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<decimal> SumWithFilter(Expression<Func<T, decimal>> column, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            return await query.SumAsync(column);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChanges();
        }
    }

}
