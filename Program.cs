using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using forex_experiment_worker.Models;

namespace forex_experiment_worker
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient _client;
            MongoServer _server;
            MongoDatabase _db;

             _client = new MongoClient("mongodb://localhost:27017");
            _server = _client.GetServer();
            _db = _server.GetDatabase("testdb");
            foreach(ForexExperiment experiment in _db.GetCollection<ForexExperiment>("experiments").FindAll())
            {
               Console.WriteLine(experiment.Name); 
               Console.WriteLine(experiment.Window.staticOptions[0]); 
            } 
            Console.WriteLine("Hello World!");
        }
    }
}
