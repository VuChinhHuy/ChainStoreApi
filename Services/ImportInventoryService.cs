using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace ChainStoreApi.Services;
public class ImportInventoryService
{
    private readonly IMongoCollection<ImportInventory> _importInventoryColection;
    public ImportInventoryService(
        IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _importInventoryColection = mongoDatabase.GetCollection<ImportInventory>(databaseSetting.Value.ImportInventoryCollectionName);
        }

    public async Task createImportInventory (ImportInventory importInventory) => await _importInventoryColection.InsertOneAsync(importInventory);
    public async Task<List<ImportInventory>> getImportInventoryStore (string idStore) => await _importInventoryColection.Find(x=>x.idStore == idStore).ToListAsync();

}