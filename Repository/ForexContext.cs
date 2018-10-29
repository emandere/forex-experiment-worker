using MongoDB.Driver;
using forex_experiment_worker.Models;
namespace forex_experiment_worker.Repository
{
    public class ForexContext
    {
        private readonly IMongoDatabase _database = null;

        public ForexContext(Settings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<ForexExperiment> Experiments
        {
            get
            {
                return _database.GetCollection<ForexExperiment>("experiments");
            }
        }

        public IMongoCollection<TradingSession> TradingSessionQueue
        {
            get
            {
                return _database.GetCollection<TradingSession>("tradingsessionqueue");
            }
        }
    }
}