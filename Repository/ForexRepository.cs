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
    }    
}