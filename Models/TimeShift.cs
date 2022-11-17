using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChainStoreApi.Models;
    public class TimeShift
    {
        public String? name {get; set;}=null!;
        public time? timeStart {get; set;}=null!;
        public time? timeEnd {get; set;}=null!;
    }
    public class time 
    {
        public int? hour {get;set;}=null!;
        public int? minute {get;set;}=null!;
    }