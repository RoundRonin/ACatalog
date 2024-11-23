using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Infrastructure;

namespace DAL;

public class EfRepository<T>(DbContext context) : IRepository<T> where T : class
{
    protected readonly DbContext _context = context;

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrUpdateAsync(T entity)
    {
        var existingEntity = await _context.Set<T>().FindAsync(GetKeyValues(entity));
        if (existingEntity == null)
        {
            await AddAsync(entity);
        }
        else
        {
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null
            ? await _context.Set<T>().ToListAsync()
            : await _context.Set<T>().Where(predicate).ToListAsync();
    }

    protected object[] GetKeyValues(T entity)
    {
        var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First();
        return [entity.GetType().GetProperty(keyProperty.Name).GetValue(entity)];
    }
}
