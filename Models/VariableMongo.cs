using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace forex_experiment_worker.Models
{
    public class VariableMongo<T>
    {
        [BsonElement("staticOptions")]
        public T[] staticOptions{get;set;}
        [BsonElement("min")]
        public T min{get;set;}
        [BsonElement("max")]
        public T max{get;set;}
        [BsonElement("increment")]
        public T increment{get;set;}
        public string name{get;set;}

        
    }
}