using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
namespace forex_experiment_worker.Models
{
    
    public class ForexExperimentMongo
    {
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("indicator")]
        public string indicator{get;set;}
        [BsonElement("startdate")]
        public string startdate{get;set;}
        [BsonElement("enddate")]
        public string enddate{get;set;}
        [BsonElement("position")]
        public string position{get;set;}
        [BsonElement("window")]
        public VariableMongo<int> window{get;set;}
        [BsonElement("units")]
        public VariableMongo<int> units{get;set;}
        [BsonElement("stoploss")]
        public VariableMongo<double> stoploss{get;set;}
        [BsonElement("takeprofit")]
        public VariableMongo<double> takeprofit{get;set;}
        public string percentcomplete{get;set;}
        public bool complete{get;set;}

        public string starttime{get;set;}
        public string endtime{get;set;}
        public string elapsedtime{get;set;}

    }
}