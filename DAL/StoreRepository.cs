using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class StoreRepository(DbContext context) : EfRepository<Store>(context), IStoreRepository
{
    public async Task<Store?> GetByIdAsync(string storeId)
    {
        return await _context.Set<Store>().FindAsync(storeId);
    }
}
