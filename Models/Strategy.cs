using MongoDB.Bson.Serialization.Attributes;
namespace forex_experiment_worker.Models
{
   public class Strategy
   {
       [BsonElement("window")]
        public int Window{get;set;}
        [BsonElement("ruleName")]
        public string RuleName{get;set;}
        [BsonElement("position")]
        public string Position{get;set;}
        [BsonElement("units")]
        public int Units{get;set;}
        [BsonElement("stopLoss")]
        public double StopLoss{get;set;}
        [BsonElement("takeProfit")]
        public double TakeProfit{get;set;}

   } 
}