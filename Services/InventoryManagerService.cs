using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace ChainStoreApi.Services;
public class InventoryManagerService
{
    private readonly IMongoCollection<InventoryManager> _inventoryManager;
    public InventoryManagerService(
        IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _inventoryManager = mongoDatabase.GetCollection<InventoryManager>(databaseSetting.Value.InventoryManagerCollectionName);
        }
    
    // create
    public async Task createCountProduct(InventoryManager inven) => await _inventoryManager.InsertOneAsync(inven);

    public async Task<List<InventoryManager>> GetAllInventory() => await _inventoryManager.Find(_ => true).ToListAsync();
    // find
    public async Task<InventoryManager?> GetInventoryManagerAsync(string idStore) => await _inventoryManager.Find(x=> x.idStore == idStore).FirstOrDefaultAsync();

    public async Task UpdateInventoryManager(string id, InventoryManager update) => await _inventoryManager.ReplaceOneAsync(x=> x.id == id ,update);
    public async Task updateCountProduct(string idStore, ImportInventory importIn)
    {
        var inven = await GetInventoryManagerAsync(idStore);
        foreach(var item in importIn.productInventory!.ToList())
        {
            foreach(var pro in inven!.productInStore!.ToList())
            {
                if(pro.product!.id == item.product!.id)
                {
                    int? countnew = pro.count + item.count; 
                    inven!.productInStore![inven.productInStore.IndexOf(pro)].count = countnew;
                    importIn!.productInventory!.Remove(item);
                    break;
                }            
            }
        }
        foreach(var item in importIn.productInventory!.ToList())
        {
            inven!.productInStore!.Add(item);
        }
        await _inventoryManager!.ReplaceOneAsync(x=> x!.idStore == idStore,inven);
        return;
    }

   

}