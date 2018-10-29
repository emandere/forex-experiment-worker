using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace forex_experiment_worker.Models
{
    public class TradingSession
    {
        [BsonElement("strategy")]
        public Strategy TradingStrategy{get;set;}
        [BsonElement("name")]
        public string Name{get;set;}
        [BsonElement("startdate")]
        public string StartDate{get;set;}
        [BsonElement("enddate")]
        public string EndDate{get;set;}
        [BsonElement("startamount")]
        public double StartAmount{get;set;}
        [BsonElement("read")]
        public bool Read{get;set;}


    }
    

}