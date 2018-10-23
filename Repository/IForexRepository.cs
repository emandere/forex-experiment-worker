using System.Threading.Tasks;
using System.Collections.Generic;
using forex_experiment_worker.Models;

namespace forex_experiment_worker.Repository
{
    public interface IForexRepository
    {
        //Task<IEnumerable<ForexExperiment>> GetAllNotes();
        Task<IEnumerable<ForexExperiment>> GetAllExperiments();
        Task AddExperiment(ForexExperiment item);
    }
}
