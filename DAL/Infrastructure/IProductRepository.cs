using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Infrastructure;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByIdAsync(string productId);
}
