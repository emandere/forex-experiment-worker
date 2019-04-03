using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using forex_experiment_worker.Models;
using forex_experiment_worker.Repository;
using forex_experiment_worker.Services;
using forex_experiment_worker.Mapper;
using forex_experiment_worker.Config;

using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
namespace forex_experiment_worker
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            


            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ForexExperimentProfile());
                cfg.AddProfile(new ForexSessionProfile());
            });



            IMapper mapper = config.CreateMapper();
             var serviceProvider = new ServiceCollection()
                .AddSingleton<IForexRepository, ForexRepository>()
                .AddTransient<ForexExperimentMap,ForexExperimentMap>()
                .AddSingleton<StrategyTestServices,StrategyTestServices>()
                .AddAutoMapper()
                .Configure<Settings>(options =>
                {
                    options.ConnectionString 
                        = configuration.GetSection("MongoConnection:ConnectionString").Value;
                    options.Database 
                        = configuration.GetSection("MongoConnection:Database").Value;
                })
                .BuildServiceProvider(); 
            

             var serv = serviceProvider.GetService<StrategyTestServices>();
             foreach(var message in await serv.ExperimentNames())
             {
                 Console.WriteLine(message);     
             }
             
           
        }        
    }
}
