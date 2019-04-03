using System;
using AutoMapper;
using forex_experiment_worker.Domain;
using forex_experiment_worker.Models;
namespace forex_experiment_worker.Config
{
    public class ForexSessionProfile:Profile
    {
        public ForexSessionProfile()
        {
            CreateMap<ForexSession, ForexSessionMongo>();
            CreateMap<ForexSessionMongo, ForexSession>()
                .ForMember
                ( dest=>dest.StartDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.StartDate).ToString("yyyy-MM-dd")
                        )
                )
                .ForMember
                (dest=>dest.EndDate,
                        opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.EndDate).ToString("yyyy-MM-dd")
                        )
                );

            CreateMap<SessionUser,SessionUserMongo>();
            CreateMap<SessionUserMongo,SessionUser>();

            CreateMap<Accounts,AccountsMongo>();
            CreateMap<AccountsMongo,Accounts>();

            CreateMap<Account,AccountMongo>();
            CreateMap<AccountMongo,Account>();

            CreateMap<BalanceHistory,BalanceHistoryMongo>();
            CreateMap<BalanceHistoryMongo,BalanceHistory>()
                .ForMember
                (
                   dest=>dest.Date, opts=>opts.MapFrom
                        (
                            src => DateTime.Parse(src.Date).ToString("yyyy-MM-dd")
                        )
                )    
            ;

            CreateMap<Trade,TradeMongo>();
            CreateMap<TradeMongo,Trade>()
                 .ForMember
                ( dest=>dest.PL, opts=>opts.MapFrom
                        (
                            src => src.PLCalc()
                        )
                       
                )
            ;

            CreateMap<Order,OrderMongo>();
            CreateMap<OrderMongo,Order>();
               
        }

    }
}