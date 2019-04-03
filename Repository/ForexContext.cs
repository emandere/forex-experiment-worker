using MongoDB.Driver;
using Microsoft.Extensions.Options;
using forex_experiment_worker.Models;
namespace forex_experiment_worker.Repository
{
    public class ForexContext
    {
        private readonly IMongoDatabase _database = null;

        public ForexContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<ForexExperimentMongo> Experiments
        {
            get
            {
                return _database.GetCollection<ForexExperimentMongo>("experiments");
            }
        }

        public IMongoCollection<TradingSession> TradingSessionQueue
        {
            get
            {
                return _database.GetCollection<TradingSession>("tradingsessionqueue");
            }
        }

        public IMongoCollection<ForexSessionMongo> ForexSessions
        {
            get
            {
                return _database.GetCollection<ForexSessionMongo>("session");
            }
        }
    }
}