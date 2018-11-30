using System.Collections.Generic;
using System.Threading.Tasks;
using forex_experiment_worker.Models;
using MongoDB.Driver;

namespace forex_experiment_worker.Repository
{
    public class ForexRepository : IForexRepository
    {
        private readonly ForexContext _context = null;

        public ForexRepository(Settings settings)
        {
            _context = new ForexContext(settings);
        }

        public async Task<IEnumerable<ForexExperiment>> GetAllExperiments()
        {
            var documents = await _context.Experiments.Find(_ => true).ToListAsync();
            return documents;
        }

        public async Task AddExperiment(ForexExperiment item)
        {
        
            await _context.Experiments.InsertOneAsync(item);
            
        }

        public async Task PushTradingStrategySession(TradingSession item)
        {
        
            await _context.TradingSessionQueue.InsertOneAsync(item);
            
        }

        public async Task<IEnumerable<ForexSession>> GetForexSessions(string experimentId)
        {
            var result = await _context.ForexSessions.Find((s)=>s.ExperimentId==experimentId).ToListAsync();
            return result;
        }

         public async Task<IEnumerable<ForexSession>> GetForexSessions()
        {
            var result = await _context.ForexSessions.Find(_=>true).ToListAsync();
            return result;
        }

    }    
}