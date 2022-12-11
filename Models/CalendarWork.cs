using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainStoreApi.Models;

public class CalendarWork
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string id {get;set;} = null!;
    public string? idStore {get;set;} =null!;
    public List<YearWork> year {get;set;} = null!;
    
    // public CalendarWork()
    // {

    // }
}
public class YearWork
{
    public string? year{get;set;}
    public List<MonthWork> month {get;set;} = null!;
}

public class MonthWork 
{
    public string? month {get;set;} = null!;

    public List<DayWork> day {get;set;} = null!;

}

public class DayWork
{
    public string? day {get;set;} = null!;   
    public List<checkWork> check {get;set;} = null!;
}

public class checkWork
{
    public TimeShift? timeShift{get;set;} =null!;

    public Staff staff {get;set;} =null!;

    public time checkStart {get;set;} = null!;

    public time checkEnd {get;set;} = null!;


}
// enum statusWork 
// {
//     late,
//     excally,
//     về sớm,
//     vắng  
// 
// }
