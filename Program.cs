﻿using System;
using System.Linq;
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
               foreach(ForexSession session in await repository.GetForexSessions(experiment.Name))
               {
                   Console.WriteLine(session.Id +" "+session
                   .SessionUser
                   .Accounts
                   .Primary
                   .BalanceHistory
                   .Last().Amount);
               }
            } 
            Console.WriteLine("Hello World!");
        }

        
    }
}
