using DAL.Entities;

namespace DAL.Infrastructure;

public interface IStoreRepository
{
    Task CreateStoreAsync(Store store);
    Task<Store> GetStoreByCodeAsync(string code);
    Task<IEnumerable<Store>> GetAllStoresAsync();
    Task UpdateStoreAsync(Store store);
    Task DeleteStoreAsync(string code);
}

public interface IProductRepository
{
    Task CreateProductAsync(Product product);
    Task<Product> GetProductByNameAsync(string name);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(string name);
}

public interface IStoreProductRepository
{
    Task AddStoreProductAsync(StoreProduct storeProduct);
    Task<IEnumerable<StoreProduct>> GetProductsByStoreAsync(string storeCode);
    Task<IEnumerable<StoreProduct>> GetStoresByProductAsync(string productName);
    Task<StoreProduct> GetStoreProductAsync(string storeCode, string productName);
    Task UpdateStoreProductAsync(StoreProduct storeProduct);
    Task DeleteStoreProductAsync(int id);
}

