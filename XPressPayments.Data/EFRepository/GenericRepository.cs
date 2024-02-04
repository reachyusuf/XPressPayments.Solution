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

        //public async Task<T> Get(Expression<Func<T, bool>> match)
        //{
        //    return await _context.Set<T>().FirstOrDefaultAsync(match);
        //}

        public async Task<T> Get(Expression<Func<T, bool>> match,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (match is not null)
            {
                query = query.Where(match);
            }
            //return await _context.Set<T>().FirstOrDefaultAsync(match);
            if (orderBy is not null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        //public async Task<T> Find(Expression<Func<T, bool>> match,
        // Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    if (match is not null)
        //    {
        //        query = query.Where(match);
        //    }
        //    if (orderBy is not null)
        //    {
        //        query = orderBy(query);
        //    }
        //    else
        //    {
        //        return await query.FirstOrDefaultAsync();
        //    }

        //    return await query.FirstOrDefaultAsync();
        //}


        public IQueryable<T> GetIQueryable(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task<T> Get()
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync();
            return result;
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, IEnumerable<string> includePaths = null)
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

        //public async Task<T> First(Expression<Func<T, bool>> filter, IEnumerable<string> includePaths = null)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    if (includePaths != null)
        //    {
        //        query = includePaths.Aggregate(query, (current, includePath) => current.Include(includePath));
        //    }

        //    return await query.SingleOrDefaultAsync(filter);
        //}

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null,
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
            //if (includeOther is not null)
            //{
            //    query = query.Include(includeOther);
            //}

            if (orderBy is not null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<T> Get(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Get(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Get(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
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


        //public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, int? page = null, Expression<Func<T, object>> sortColumn = null, SortDirections sortDirection = SortDirections.Ascending)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    if (filter is not  null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    if (page is not  null)
        //    {
        //        query = query.Skip((page.Value - 1) * _pageSize).Take(_pageSize);
        //    }

        //    if (sortColumn is not  null)
        //    {
        //        if (sortDirection == SortDirections.Ascending)
        //            query = query.OrderBy(sortColumn);
        //        else
        //            query = query.OrderByDescending(sortColumn);
        //    }

        //    return await query.ToListAsync();
        //}

        //public IEnumerable<T> GetAllWithTotalRows(out int totalRows, Expression<Func<T, bool>> filter = null, int? page = null, Expression<Func<T, object>> sortColumn = null, SortDirections sortDirection = SortDirections.Ascending)
        //{
        //    IQueryable<T> query = _context.Set<T>();

        //    if (sortColumn is not  null)
        //    {
        //        if (sortDirection == SortDirections.Ascending)
        //            query = query.OrderBy(sortColumn);
        //        else
        //            query = query.OrderByDescending(sortColumn);
        //    }

        //    if (filter is not  null)
        //    {
        //        query = query.Where(filter);
        //        totalRows = query.Count(filter);
        //    }
        //    else
        //    {
        //        totalRows = query.Count();
        //    }

        //    if (page is not  null)
        //    {
        //        query = query.Skip((page.Value - 1) * _pageSize).Take(_pageSize);
        //    }

        //    return query.ToList();
        //}

        public Task<IQueryable<T>> GetAsNoTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return Task.Run(() =>
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
                if (includeProperties is not null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty).AsNoTracking();
                    }
                }
                if (page is not null && pageSize is not null)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
                return query;
            });
        }

        public Task<IQueryable<T>> GetAsNoTracking(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return Task.Run(() =>
            {
                IQueryable<T> query = _context.Set<T>().AsNoTracking();
                if (orderBy is not null)
                {
                    query = orderBy(query);
                }
                if (includeProperties is not null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty).AsNoTracking();
                    }
                }
                return query;
            });
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string includeProperties = "", int? page = null, int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            if (includeProperties is not null)
            {
                foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim()))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (page is not null && pageSize is not null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query.ToList();
        }


        //--
        public IQueryable<T> PaginatedListAsNoTracking(IQueryable<T> source, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    int? page = null, int? pageSize = null, params Expression<Func<T, object>>[] includeProperties)
        {
            pageSize = pageSize ?? _pageSize;
            if (orderBy is not null)
            {
                source = orderBy(source);
            }
            if (includeProperties is not null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    source = source.Include(includeProperty).AsNoTracking();
                }
            }
            if (page is not null)
            {
                source = source.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return source;
        }
        //--
        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChanges();
        }
    }

}
