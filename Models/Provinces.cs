using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChainStoreApi.Models;

public class Provinces
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? _id {get;set;}
    public string? name {get;set;}
    public int? code{get;set;}
    public string? codename {get;set;}
    public string? division_type {get;set;}
    public int? phone_code {get;set;}
    public List<districts>? districts {get;set;} 
}
public class wards 
{
    public string? name {get;set;}
    public int? code{get;set;}
    public string? codename {get;set;}
    public string? division_type {get;set;}
    public string? short_codename {get;set;}
}
public class districts 
{
    public string? name {get;set;}
    public int? code{get;set;}
    public string? codename {get;set;}
    public string? division_type {get;set;}
    public string? short_codename {get;set;}
    public List<wards>? wards {get;set;} 

}
