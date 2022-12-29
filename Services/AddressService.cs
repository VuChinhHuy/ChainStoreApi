using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChainStoreApi.Services;

public class AddressService
{
    private readonly IMongoCollection<Provinces> _addressCollection;
    public AddressService(IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _addressCollection = mongoDatabase.GetCollection<Provinces>(databaseSetting.Value.ProvincesCollectionName);
        }
   public async Task<List<Provinces>> getProvinces() => await _addressCollection.Find(_=>true).ToListAsync();
}