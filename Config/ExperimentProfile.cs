using AutoMapper;
using forex_experiment_worker.Domain;
using forex_experiment_worker.Models;
namespace forex_experiment_worker.Config
{
    public class ForexExperimentProfile:Profile
    {
        public ForexExperimentProfile()
        {
            CreateMap<ForexExperiment, ForexExperimentMongo>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ForexExperimentMongo, ForexExperiment>()
                .ForMember(x => x.sessions, opt => opt.Ignore());
        }

    }
}