using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class InventoryManager
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? id {get;set;}

    public string? idStore {get;set;}

    public List<ProductInInventory>? productInStore {get;set;}

    public InventoryManager(string idStore, List<ProductInInventory> product){
        this.idStore = idStore;
        this.productInStore = product;
    }
    public InventoryManager()
    {
        
    }
}