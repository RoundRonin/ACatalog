using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class FileStoreRepository(string filePath) : IStoreRepository
{
    private readonly string _filePath = filePath;

    public async Task AddOrUpdateAsync(Store entity)
    {
        var stores = (await GetAllAsync()).ToList();
        var index = stores.FindIndex(line => line.Id == entity.Id);

        if (index >= 0)
        {
            stores[index] = entity;
        } else
        {
            stores.Add(entity);
        }

        var lines = stores.Select(s => $"{s.Id},{s.Name},{s.Address}");
        await FileHelper.WriteLinesAsync(_filePath, lines, append: false);
    }

    public async Task UpdateAsync(Store entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task<Store?> GetByIdAsync(int id)
    {
        var stores = await GetAllAsync();
        return stores.FirstOrDefault(s => s.Id == id.ToString());
    }

    public async Task<IEnumerable<Store>> GetAllAsync(System.Linq.Expressions.Expression<Func<Store, bool>>? predicate = null)
    {
        var lines = await FileHelper.ReadLinesAsync(_filePath);
        var stores = lines.Select(line =>
        {
            var parts = line.Split(',');
            return new Store
            {
                Id = parts[0],
                Name = parts[1],
                Address = parts[2]
            };
        });

        return predicate == null ? stores : stores.Where(predicate.Compile());
    }

    public async Task<Store?> GetByIdAsync(string storeId)
    {
        var stores = await GetAllAsync();
        return stores.FirstOrDefault(s => s.Id == storeId);
    }
}
