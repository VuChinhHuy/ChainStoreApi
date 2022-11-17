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

    public List<ProductInInventory>? product {get;set;}
}