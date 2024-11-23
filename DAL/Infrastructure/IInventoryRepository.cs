using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Infrastructure;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<IEnumerable<StoreInventory>> GetAllProductsInAStore(Expression<Func<Inventory, bool>> predicate);
}

