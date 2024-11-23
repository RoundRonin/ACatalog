using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class InventoryRepository(DbContext context) : EfRepository<Inventory>(context), IInventoryRepository
{
    public async Task<IEnumerable<StoreInventory>> GetAllProductsInAStore(Expression<Func<Inventory, bool>> predicate)
    {
        return await _context.Set<Inventory>()
            .Where(predicate)
            .Select(i => new StoreInventory
            {
                ProductName = _context.Set<Product>().FirstOrDefault(p => p.Id == i.ProductId).Name,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            })
            .ToListAsync();
    }
}
