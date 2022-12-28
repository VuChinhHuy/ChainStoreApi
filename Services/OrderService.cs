using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace ChainStoreApi.Services;

public class OrderService
{
    private readonly IMongoCollection<Order> _OrderCollection;

    private readonly IMongoCollection<InventoryManager> _importMangagerCollection;
    public OrderService(
        IOptions<DatabaseSetting> databaseSetting)
    {
        var mongoClient = new MongoClient(
            databaseSetting.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

        _OrderCollection = mongoDatabase.GetCollection<Order>(databaseSetting.Value.OrderCollectionName);
        _importMangagerCollection = mongoDatabase.GetCollection<InventoryManager>(databaseSetting.Value.InventoryManagerCollectionName);
    }

    public async Task<List<Order>> GetOrderAsync()
    {
        var data = await _OrderCollection.Find(_ => true).ToListAsync();
        return data;
    }
    public async Task<Order?> GetOrderAsync(string id) => await _OrderCollection.Find(x => x.id == id).FirstOrDefaultAsync();

    public async Task CreateOrderAsync(Order Order) {
        
        await _OrderCollection.InsertOneAsync(Order);

        var productIn = await _importMangagerCollection.Find(x=> x.idStore == Order.OrderStaff.storeId).FirstOrDefaultAsync();

        foreach (var pro in Order.OrderDetails.ToList())
        {
            foreach (var item in productIn!.productInStore!.ToList())
            {
                if(pro.Product!.id == item.product!.id )
                {
                    item.count = item.count - pro.count;
                    Order.OrderDetails.Remove(pro);
                    break;
                }
            }
        }
        await _importMangagerCollection.ReplaceOneAsync(x=>x.id == productIn.id,productIn);

    } 
    public async Task UpdateOrderAsync(string id ,Order Order) {
        await RemoveOrderAsync(id);
        Order.id = id;
        await CreateOrderAsync(Order);
    }
    
    public async Task RemoveOrderAsync(string id) {

        var order = await _OrderCollection.Find(x=> x.id == id).FirstOrDefaultAsync();
        var productIn = await _importMangagerCollection.Find(x=> x.idStore == order.OrderStaff.storeId).FirstOrDefaultAsync();
        foreach (var pro in order.OrderDetails.ToList())
        {
            foreach (var item in productIn.productInStore!.ToList())
            {
                if(pro.Product!.id == item.product!.id )
                {
                    item.count = item.count + pro.count;
                    order.OrderDetails.Remove(pro);
                    break;
                }
            }
        }
        await _importMangagerCollection.ReplaceOneAsync(x=>x.id == productIn.id,productIn);

        await _OrderCollection.DeleteOneAsync(x => x.id == id);
    } 

    public async Task<List<Order>> SearchKeyword(string? startDate, string? endDate, string? keyword)
    {       
        var query = await _OrderCollection.Find(_ => true).ToListAsync();
        if (!string.IsNullOrEmpty(startDate))
        {
            query = query.Where(x =>
            DateTime.Parse( DateTime.Parse(x.OrderDate + string.Empty).ToString("dd/MM/yyyy")) >= DateTime.Parse( DateTime.Parse(startDate + string.Empty).ToString("dd/MM/yyyy"))).ToList();
        }

        if (!string.IsNullOrEmpty(endDate))
        {
            query = query.Where(x =>
            DateTime.Parse( DateTime.Parse(x.OrderDate + string.Empty).ToString("dd/MM/yyyy")) <= DateTime.Parse( DateTime.Parse(endDate + string.Empty).ToString("dd/MM/yyyy"))).ToList();
        }
        if (!string.IsNullOrEmpty(keyword))
        {
            
            query = query.Where(x => (x.customer.first_name + x.customer.last_name ).Contains(keyword) ||
                            x.customer.phone.Contains(keyword)
                            ).ToList();
        }
        return query;
    }
}