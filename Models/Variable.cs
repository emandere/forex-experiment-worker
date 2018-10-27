using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
namespace forex_experiment_worker.Models
{
    public abstract class Variable
    {
        public abstract List<Strategy> CartesianProduct(List<Strategy> currentProduct);
    }
    public class Variable<T>
    {
        [BsonElement("staticOptions")]
        public T[] staticOptions{get;set;}
        [BsonElement("min")]
        public T min{get;set;}
        [BsonElement("max")]
        public T max{get;set;}
        [BsonElement("increment")]
        public T increment{get;set;}
        public string name{get;set;}

        IEnumerable<T> options()
        { 
            
            if(staticOptions!=null)
            {
                return staticOptions;
            }
            List<T> returnList = new List<T>();
            for (dynamic i = min; i < max; i += increment)
            {
                returnList.Add(i);
            } 
            return returnList;   
        }
        Strategy createStrategy(Strategy oldStrategy, dynamic currentValue)
        {
            Strategy newStrategy = new Strategy();

            newStrategy.Window=oldStrategy.Window;
            newStrategy.StopLoss = oldStrategy.StopLoss;
            newStrategy.TakeProfit = oldStrategy.TakeProfit;
            newStrategy.Position =oldStrategy.Position;
            newStrategy.RuleName =oldStrategy.RuleName;
            newStrategy.Units = oldStrategy.Units;

            switch(name)
            {
            case "Window":
                newStrategy.Window=currentValue;
                break;
            case "StopLoss":
                newStrategy.StopLoss=currentValue;
                break;
            case "TakeProfit" :
                newStrategy.TakeProfit=currentValue;
                break;
            case "Units":
                newStrategy.Units=currentValue;
                break;
            case "RuleName":
                newStrategy.RuleName = currentValue;
                break;
            case "Position":
                newStrategy.Position = currentValue;
                break;
            }
            return newStrategy;
        }
        public List<Strategy> CartesianProduct(List<Strategy> currentProduct)
        {
            var returnNewList = new List<Strategy>();
            if(currentProduct.Count==0)
            {
                foreach(var currentValue in options())
                {
                    returnNewList.Add(createStrategy(new Strategy(), currentValue));
                }
                
            }

            return returnNewList;
        }
    }
}