using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace forex_experiment_worker.Models
{
    public  class ForexSession
    {
        //[BsonElement("_id")]
        //public string _Id { get; set; }

        //[BsonElement("id")]
        //public string id { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string idinfo { get; set; }

        [BsonElement("sessionType")]
        public string SessionType { get; set; }

        [BsonElement("experimentId")]
        public string ExperimentId { get; set; }

        [BsonElement("startDate")]
        public string StartDate { get; set; }

        [BsonElement("endDate")]
        public string EndDate { get; set; }

        [BsonElement("lastUpdatedTime")]
        public string LastUpdatedTime { get; set; }

        [BsonElement("currentTime")]
        public string CurrentTime { get; set; }

        [BsonElement("strategy")]
        public Strategy Strategy { get; set; }

        [BsonElement("sessionUser")]
        public SessionUser SessionUser { get; set; }

        [BsonElement("percentComplete")]
        public string PercentComplete { get; set; }
    }

    public  class SessionUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string idinfo { get; set; }

        [BsonElement("status")]
        public object Status { get; set; }

        [BsonElement("Accounts")]
        public Accounts Accounts { get; set; }
    }

    public  class Accounts
    {
        
        [BsonElement("primary")]
        public Account Primary { get; set; }

        [BsonElement("secondary")]
        public Account Secondary { get; set; }
    }

    public  class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("id")]
        public string idinfo { get; set; }

        [BsonElement("cash")]
        public double Cash { get; set; }

        [BsonElement("Margin")]
        public long Margin { get; set; }

        [BsonElement("MarginRatio")]
        public long MarginRatio { get; set; }

        [BsonElement("realizedPL")]
        public double RealizedPl { get; set; }

        [BsonElement("Trades")]
        public Trade[] Trades { get; set; }

        [BsonElement("orders")]
        public Order[] Orders { get; set; }

        [BsonElement("closedTrades")]
        public Trade[] ClosedTrades { get; set; }

        [BsonElement("balanceHistory")]
        public BalanceHistory[] BalanceHistory { get; set; }

        [BsonElement("idcount")]
        public long Idcount { get; set; }
    }

    public  class BalanceHistory
    {
        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }
    }

    public  class Trade
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public long? Id { get; set; }

        [BsonElement("id")]
        public long? idinfo { get; set; }

        [BsonElement("pair")]
        public string Pair { get; set; }

        [BsonElement("units")]
        public long Units { get; set; }

        [BsonElement("openDate")]
        public string OpenDate { get; set; }

        [BsonElement("closeDate")]
        public string CloseDate { get; set; }

        [BsonElement("long")]
        public bool Long { get; set; }

        [BsonElement("openPrice")]
        public double OpenPrice { get; set; }

        [BsonElement("closePrice")]
        public double ClosePrice { get; set; }

        [BsonElement("stopLoss")]
        public double? StopLoss { get; set; }

        [BsonElement("takeProfit")]
        public double? TakeProfit { get; set; }


        [BsonElement("init")]
        public bool Init { get; set; }
    }

    public  class Order
    {
        [BsonElement("expirationDate")]
        public object ExpirationDate { get; set; }

        [BsonElement("triggerprice")]
        public double Triggerprice { get; set; }

        [BsonElement("expired")]
        public bool Expired { get; set; }

        [BsonElement("above")]
        public bool Above { get; set; }

        [BsonElement("trade")]
        public Trade Trade { get; set; }
    }

}
