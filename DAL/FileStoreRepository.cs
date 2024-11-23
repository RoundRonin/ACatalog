using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Infrastructure;

namespace DAL;

public class FileStoreRepository(string filePath) : IRepository<Store>
{
    private readonly string _filePath = filePath;

    public async Task AddAsync(Store entity)
    {
        var lines = new List<string> { $"{entity.Id},{entity.Name}" };
        await FileHelper.WriteLinesAsync(_filePath, lines, append: true);
    }

    public async Task AddOrUpdateAsync(Store entity)
    {
        var stores = await GetAllAsync();
        var storeList = stores.ToList();

        var store = storeList.FirstOrDefault(s => s.Id == entity.Id);
        if (store != null)
        {
            storeList.Remove(store);
        }
        storeList.Add(entity);

        var lines = storeList.Select(s => $"{s.Id},{s.Name}");
        await FileHelper.WriteLinesAsync(_filePath, lines, append: false);
    }

    public async Task UpdateAsync(Store entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task<Store> GetByIdAsync(int id)
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
                Name = parts[1]
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
