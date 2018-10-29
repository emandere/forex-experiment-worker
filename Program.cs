using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using forex_experiment_worker.Models;
using forex_experiment_worker.Repository;

namespace forex_experiment_worker
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            Settings settings = new Settings();//configuration.GetConnectionString("Storage"));
            settings.ConnectionString= configuration.GetSection("MongoConnection:ConnectionString").Value;
            settings.Database = configuration.GetSection("MongoConnection:Database").Value;

            ForexRepository repository = new ForexRepository(settings);
           
            foreach(ForexExperiment experiment in await repository.GetAllExperiments())
            {
               Console.WriteLine(experiment.Name); 
               List<Strategy> x2 = experiment.GetStrategies();
               foreach(Strategy s in x2)
               {
                   TradingSession session = new TradingSession();
                   session.Name = experiment.Name+"_0";
                   session.StartDate = experiment.StartDate;
                   session.EndDate = experiment.EndDate;
                   session.TradingStrategy = s;
                   session.Read = false;
                   session.StartAmount = 2000.0;
                   await repository.PushTradingStrategySession(session);
               }
               Console.WriteLine(x2.Count);
            } 
            Console.WriteLine("Hello World!");
        }

        
    }
}
