using MongoDB.Bson.Serialization.Attributes;
namespace forex_experiment_worker.Models
{
    public class Variable
    {
        [BsonElement("staticOptions")]
        public string[] staticOptions;
        [BsonElement("min")]
        public string min;
        [BsonElement("max")]
        public string max;
        [BsonElement("increment")]
        public string increment;
    }
}