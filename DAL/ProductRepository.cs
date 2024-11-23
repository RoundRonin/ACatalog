using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class ProductRepository(DbContext context) : EfRepository<Product>(context), IProductRepository
{
}
