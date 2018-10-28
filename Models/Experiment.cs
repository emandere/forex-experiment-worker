using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
namespace forex_experiment_worker.Models
{
   
    public class ForexExperiment
    {
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("indicator")]
        public string Indicator{get;set;}
        [BsonElement("startdate")]
        public string StartDate{get;set;}
        [BsonElement("enddate")]
        public string EndDate{get;set;}
        [BsonElement("position")]
        public string Position{get;set;}
        [BsonElement("window")]
        public Variable<int> Window{get;set;}
        [BsonElement("units")]
        public Variable<int> Units{get;set;}
        [BsonElement("stoploss")]
        public Variable<double> StopLoss{get;set;}
        [BsonElement("takeprofit")]
        public Variable<double> TakeProfit{get;set;}

        public List<Strategy> GetStrategies()
        {
            List<Variable> variables = new List<Variable>();
            Window.name="Window";
            StopLoss.name ="StopLoss";
            TakeProfit.name ="TakeProfit";
            Units.name ="Units";
            
            Variable<string> position = new Variable<string>();
            position.name="Position";
            position.staticOptions= new string[]{Position};

            Variable<string> RuleName = new Variable<string>();
            RuleName.name="RuleName";
            RuleName.staticOptions= new string[]{Indicator};

            variables.Add(Window);
            variables.Add(StopLoss);
            variables.Add(TakeProfit);
            variables.Add(Units);
            variables.Add(position);
            variables.Add(RuleName);
            

            return GetStrategyHelper(variables);
        }

        public List<Strategy> GetStrategyHelper(List<Variable> variables)
        {
            if(variables.Count==1)
            {
                return variables[0].CartesianProduct(new List<Strategy>());
            }
            else
            {
                return variables[0].CartesianProduct(GetStrategyHelper(variables.Skip(1).ToList()));
            }
        }

    }
}