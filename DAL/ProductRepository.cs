using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class ProductRepository(DbContext context) : EfRepository<Product>(context), IProductRepository
{
    public async Task<Product?> GetByIdAsync(string storeId)
    {
        return await _context.Set<Product>().FindAsync(storeId);
    }
}
