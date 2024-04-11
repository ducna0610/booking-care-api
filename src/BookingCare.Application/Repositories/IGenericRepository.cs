using BookingCare.Application.DTOs.Responses;
using System.Linq.Expressions;

namespace BookingCare.Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> AnyAsync(IEnumerable<Expression<Func<T, bool>>>? filters = null);
        bool Any(IEnumerable<Expression<Func<T, bool>>>? filters = null);
        Task<bool> AnyAsync();
        bool Any();
        Task<int> CountAsync(IEnumerable<Expression<Func<T, bool>>>? filters = null);
        int Count(IEnumerable<Expression<Func<T, bool>>>? filters = null);
        Task<T> GetByIdAsync(object id);
        T GetById(object id);
        Task<IEnumerable<T>> GetAsync(
            IEnumerable<Expression<Func<T, bool>>>? filters = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null);
        Task<PaginationResponse<T>> ToPaginationAsync(
            int pageIndex = 0,
            int pageSize = 10,
            IEnumerable<Expression<Func<T, bool>>>? filters = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            IEnumerable<Func<IQueryable<T>, IOrderedQueryable<T>>>? orderBy = null);
        Task<T> FirstOrDefaultAsync(
            IEnumerable<Expression<Func<T, bool>>>? filters = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task Delete(object id);
    }
}