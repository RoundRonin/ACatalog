using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Infrastructure;

public interface IStoreRepository : IRepository<Store>
{
    Task<Store?> GetByIdAsync(string storeId);
}
