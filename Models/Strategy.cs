namespace forex_experiment_worker.Models
{
   public class Strategy
   {
        public int Window{get;set;}
        public string RuleName{get;set;}
        public string Position{get;set;}
        public int Units{get;set;}
        public double StopLoss{get;set;}
        public double TakeProfit{get;set;}

   } 
}