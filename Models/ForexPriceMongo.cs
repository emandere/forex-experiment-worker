using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace forex_experiment_worker.Models
{
    public class ForexPriceMongo
    {
        [BsonElement("_id")]
        public string id { get; set; }
        [BsonElement("pair")]
        public string pair { get; set; }
        [BsonElement("date")]
        public string date { get; set; }
        [BsonElement("open")]
        public double open { get; set; }
        [BsonElement("high")]
        public double high { get; set; }
        [BsonElement("low")]
        public double low { get; set; }
        [BsonElement("close")]
        public double close { get; set; }
        [BsonElement("datetime")]
        public DateTime datetime { get; set; }



    }
}