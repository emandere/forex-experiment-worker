using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using forex_experiment_worker.Repository;
using forex_experiment_worker.Domain;
using forex_experiment_worker.Models;



namespace forex_experiment_worker.Mapper
{
    public class ForexExperimentMap
    {
        private readonly IMapper _mapper;
    
        private readonly ForexContext _context = null;

       
        public ForexExperimentMap(IMapper mapper,IOptions<Settings> settings)
        {
            _mapper = mapper;
            _context = new ForexContext(settings);;
        }

        public async Task<List<ForexSessionMongo>> GetForexSessions(string experimentId)
        {
            var result = await _context.ForexSessions.Find((s)=>s.ExperimentId==experimentId).ToListAsync();
            return result;
        }

        public async Task<ForexSessionMongo> GetForexSessionMongo(string sessionId)
        {
            var result = await _context.ForexSessions.Find((s)=>s.Id==sessionId).SingleOrDefaultAsync();
            return result;
        }

        public async Task<ForexSession> GetForexSession(string sessionId)
        {
            var result = _mapper.Map<ForexSession>(await GetForexSessionMongo(sessionId));
            return result;
        }


        public async Task<IEnumerable<ForexExperiment>> GetExperiments()
        {
            var experimentsMongo = await _context.Experiments.Find(_ => true).ToListAsync();
            var experiments = experimentsMongo.Select((exp)=>_mapper.Map<ForexExperiment>(exp)).ToList();
            foreach(ForexExperiment experiment in experiments)
            {
                var sessions = await GetForexSessions(experiment.name);
                var sessionsCount = sessions.Count;
                var sessionsCompleteCount = sessions.FindAll((x)=>double.Parse(x.PercentComplete) >= 100).Count;
                var totalCount = experiment.GetStrategies().Count;
                double percentcomplete = ((double) sessionsCompleteCount / (double) totalCount)*100;
                experiment.percentcomplete = percentcomplete.ToString();
                if(percentcomplete>=100.0)
                {
                    experiment.complete =true;
                    //int totaltime = sessions.Select((x)=>int.Parse(x.elapsedTime)).Sum();
                    DateTime startTime = sessions.Select((x)=>DateTime.Parse(x.beginSessionTime)).Min();
                    DateTime endTime = sessions.Select((x)=>DateTime.Parse(x.endSessionTime)).Max();
                    experiment.elapsedtime= ((TimeSpan)(endTime -startTime)).TotalSeconds.ToString();
                    /* if(string.IsNullOrEmpty(experiment.endtime))
                    {
                        experiment.endtime =DateTime.Now.ToLongTimeString();
                        DateTime startTime = DateTime.Parse(experiment.starttime);
                        DateTime endTime = DateTime.Parse(experiment.endtime);
                        experiment.elapsedtime= ((TimeSpan)(endTime -startTime)).Minutes.ToString();
                        await SaveExperimentTime(experiment);
                        
                    }*/

                    //DateTime startTime2 = DateTime.Parse(experiment.starttime);
                    //DateTime endTime2 = DateTime.Parse(experiment.endtime);
                    //experiment.elapsedtime= ((TimeSpan)(endTime2 -startTime2)).Minutes.ToString();
                }
                else
                {
                    experiment.complete=false;
                    experiment.endtime = string.Empty;
                    experiment.elapsedtime =string.Empty;
                }
                    
                
                foreach(ForexSessionMongo session in sessions)
                {
                    double firstBalance = session
                                            .SessionUser
                                            .Accounts
                                            .Primary
                                            .BalanceHistory
                                            .First().Amount;

                   double lastBalance = session
                                            .SessionUser
                                            .Accounts
                                            .Primary
                                            .BalanceHistory
                                            .Last().Amount;

                   experiment.sessions.Add(new SessionAnalysis
                                        {
                                            PL=lastBalance - firstBalance,
                                            SessionStrategy = _mapper.Map<Strategy>(session.Strategy),
                                            Id = session.Id
                                        });


                }

                experiment.sessions = experiment.sessions.OrderBy(o=>o.SessionStrategy.stopLoss)
                                   .ThenBy(o=>o.SessionStrategy.takeProfit)
                                   .ToList();
            }
            return experiments;
        }

        public async Task AddExperiment(ForexExperimentMongo item)
        {
        
            await _context.Experiments.InsertOneAsync(item);
            
        }

         public async Task SaveExperimentTime(ForexExperiment experiment)
        {
            var expMongo = _mapper.Map<ForexExperimentMongo>(experiment);
            var filter = Builders<ForexExperimentMongo>.Filter.Eq(exp => exp.name, expMongo.name);
            await _context.Experiments.ReplaceOneAsync(filter, expMongo, new UpdateOptions {IsUpsert = true});


            /* await _context.Experiments.FindOneAndUpdateAsync<ForexExperimentMongo>(
                                e=>e.name==expMongo.name,
                                Builders<ForexExperimentMongo>.Update.Set(e=>e.endtime,experiment.endtime)
                                );
            await _context.Experiments.FindOneAndUpdateAsync<ForexExperimentMongo>(
                                e=>e.name==item.name,
                                Builders<ForexExperimentMongo>.Update.Set(e=>e.elapsedtime,experiment.elapsedtime)
                                );*/
            
        }

        public async Task PushTradingStrategySession(TradingSession item)
        {
        
            await _context.TradingSessionQueue.InsertOneAsync(item);
            
        }

        public async Task<IEnumerable<TradingSession>> GetAllQueuedSessions()
        {
            List<TradingSession> tradingSessions = await _context.TradingSessionQueue.Find(x=>x.Read==true).ToListAsync();
            foreach(TradingSession tradingSession in tradingSessions)
            {
                ForexSessionMongo session = await GetForexSessionMongo(tradingSession.Name);
                if(session!=null)
                    tradingSession.percentcomplete = session.PercentComplete;
            }
            return  tradingSessions;
        }

        public async Task<IEnumerable<TradingSession>> GetInProcessSessions(string experimentId)
        {
            return await _context.TradingSessionQueue
                                 .Find(x=>x.Read==false && x.ExperimentId==experimentId)
                                 .ToListAsync();
        }

        
        public async Task<IEnumerable<TradingSession>> GetAllQueuedSessions(string experimentId)
        {
            List<TradingSession> tradingSessions = await _context.TradingSessionQueue
                                                                 .Find(x=>x.Read==false && x.ExperimentId==experimentId)
                                                                 .ToListAsync();
            foreach(TradingSession tradingSession in tradingSessions)
            {
                ForexSessionMongo session = await GetForexSessionMongo(tradingSession.Name);
                if(session!=null)
                    tradingSession.percentcomplete = session.PercentComplete;
            }
            return  tradingSessions;
        }



        public async Task<IEnumerable<TradingSession>> GetInProcessSessions()
        {
            return await _context.TradingSessionQueue.Find(x=>x.Read==false).ToListAsync();
        }

        

        public async Task DeleteExperiment(string name)
        {
            await _context.Experiments.DeleteOneAsync(item=>item.name==name);
        }

        public async Task<string> CreateExperiment(ForexExperiment experiment)
        {
            experiment.starttime =DateTime.Now.ToLongTimeString();
            await AddExperiment(_mapper.Map<ForexExperimentMongo>(experiment));
            List<Strategy> _strategies = experiment.GetStrategies();
            int counter = 0;
            foreach(Strategy _strategy in _strategies)
            {
                TradingSession session = new TradingSession();
                session.Name = $"{experiment.name}_{counter}";
                session.StartDate = experiment.startdate;
                session.EndDate = experiment.enddate;
                session.TradingStrategy = _mapper.Map<StrategyMongo>(_strategy);
                session.Read = false;
                session.ExperimentId = experiment.name;
                session.StartAmount = experiment.startamount;
                await PushTradingStrategySession(session);
                counter++;
            }

            return $"{experiment.name} added";
        }

    }

     
}