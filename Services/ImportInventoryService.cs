using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace ChainStoreApi.Services;
public class ImportInventoryService
{
    private readonly InventoryManagerService _inventory;
    private readonly IMongoCollection<ImportInventory> _importInventoryColection;
    public ImportInventoryService(
        IOptions<DatabaseSetting> databaseSetting, InventoryManagerService inventoryManagerService)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _importInventoryColection = mongoDatabase.GetCollection<ImportInventory>(databaseSetting.Value.ImportInventoryCollectionName);
            _inventory = inventoryManagerService;
        }

    public async Task createImportInventory (ImportInventory importInventory)
    {
        await _importInventoryColection.InsertOneAsync(importInventory);
        var inventory = await _inventory.GetInventoryManagerAsync(importInventory.idStore!);
        if( inventory is null)
        {
            var invenNew = new InventoryManager(importInventory.idStore!,importInventory.productInventory!);
            await _inventory.createCountProduct(invenNew);
        }
        else
         await _inventory.updateCountProduct(importInventory.idStore!,importInventory);

        
    }
    public async Task<List<ImportInventory>> getImportInventoryStore (string idStore) => await _importInventoryColection.Find(x=>x.idStore == idStore).ToListAsync();

    public async Task<InventoryManager?> getProductInStore (string idStore) => await _inventory!.GetInventoryManagerAsync(idStore);

    // public async Task<List<Stor>> getProductInStoreDiff(string idProduct) {

    // }

}