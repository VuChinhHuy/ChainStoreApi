using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class ImportInventory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? id {get;set;} = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string? idStore {get;set;}

    public List<ProductInInventory>? productInventory {get;set;}
    
    public DateTime? create_at {get;set;}= null!;
    public DateTime? update_at {get;set;}= null!;
    public string create_user {get;set;}=null!;
    public string update_user {get;set;}=null!;
}

public class ProductInInventory
{
    public Product? product {get;set;}

    public int? count {get;set;}
}