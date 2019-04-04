using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forex_experiment_worker.Mapper;
namespace forex_experiment_worker.Services
{
    public class StrategyTestServices
    {
        private readonly ForexExperimentMap _forexExperimentMap;
        
        public StrategyTestServices(ForexExperimentMap forexExperimentMap)
        {
            _forexExperimentMap = forexExperimentMap;
        }
        public async Task<IEnumerable<string>> ExperimentNames()
        {
            var experimentsMongo =  await _forexExperimentMap.GetExperiments();
            return experimentsMongo.Select(x=>x.name);
        }

        public async Task<IEnumerable<string>> ForexPrices()
        {
            var pricesMongo = await _forexExperimentMap.GetPrices();
            return pricesMongo.Select(x=>x.datetime.ToString());
        }

    }

}