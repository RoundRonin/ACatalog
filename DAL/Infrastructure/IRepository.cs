using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Infrastructure;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task AddOrUpdateAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
}