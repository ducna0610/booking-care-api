using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.Repositories;
using BookingCare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookingCare.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected BookingCareDbContext _context;
        protected DbSet<T> _dbSet;
        public GenericRepository(BookingCareDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        #region  Read

        public async Task<bool> AnyAsync(IEnumerable<Expression<Func<T, bool>>>? filters)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            return await dbQuery.AnyAsync();
        }
        public bool Any(IEnumerable<Expression<Func<T, bool>>>? filters)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            return dbQuery.Any();
        }

        public async Task<bool> AnyAsync()
            => await _dbSet.AnyAsync();

        public bool Any()
            => _dbSet.Any();

        public async Task<int> CountAsync(IEnumerable<Expression<Func<T, bool>>>? filters)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            return await dbQuery.CountAsync();
        }
        public int Count(IEnumerable<Expression<Func<T, bool>>>? filters)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            return dbQuery.Count();
        }

        public async Task<T> GetByIdAsync(object id)
            => await _dbSet.FindAsync(id);

        public T GetById(object id)
            => _dbSet.Find(id);

        public async Task<IEnumerable<T>> GetAsync(
            IEnumerable<Expression<Func<T, bool>>>? filters,
            IEnumerable<Expression<Func<T, object>>>? includes)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            if (includes != null)
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    dbQuery = dbQuery.Include<T, object>(include);
                }
            }


            return await dbQuery.AsNoTracking().ToListAsync();
        }

        public async Task<PaginationResponse<T>> ToPaginationAsync(
            int pageIndex,
            int pageSize,
            IEnumerable<Expression<Func<T, bool>>>? filters,
            IEnumerable<Expression<Func<T, object>>>? includes,
            IEnumerable<Func<IQueryable<T>, IOrderedQueryable<T>>>? orders = null)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            if (includes != null)
            {

                foreach (Expression<Func<T, object>> include in includes)
                {
                    dbQuery = dbQuery.Include<T, object>(include);
                }
            }

            if (orders != null)
            {
                foreach (Func<IQueryable<T>, IOrderedQueryable<T>> order in orders.Reverse())
                {
                    dbQuery = order(dbQuery);
                }
            }

            var itemCount = dbQuery.Count();

            var items = await dbQuery.Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new PaginationResponse<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<T> FirstOrDefaultAsync(
            IEnumerable<Expression<Func<T, bool>>>? filters,
            IEnumerable<Expression<Func<T, object>>>? includes)
        {
            IQueryable<T> dbQuery = _dbSet;

            if (filters != null)
            {
                foreach (Expression<Func<T, bool>> filter in filters)
                {
                    dbQuery = dbQuery.Where(filter);
                }
            }

            if (includes != null)
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    dbQuery = dbQuery.Include<T, object>(include);
                }
            }

            return await dbQuery.IgnoreQueryFilters()
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Update & delete

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        public async Task Delete(object id)
        {
            T entity = await GetByIdAsync(id);
            Delete(entity);
        }
        #endregion
    }
}