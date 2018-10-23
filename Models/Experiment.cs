using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace forex_experiment_worker.Models
{
   
    public class ForexExperiment
    {
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("indicator")]
        public string Indicator{get;set;}
        [BsonElement("startdate")]
        public string StartDate{get;set;}
        [BsonElement("enddate")]
        public string EndDate{get;set;}
        [BsonElement("position")]
        public string Position{get;set;}
        [BsonElement("window")]
        public Variable Window{get;set;}
        [BsonElement("units")]
        public Variable Units{get;set;}
        [BsonElement("stoploss")]
        public Variable StopLoss{get;set;}
        [BsonElement("takeprofit")]
        public Variable TakeProfit{get;set;}
    }
}