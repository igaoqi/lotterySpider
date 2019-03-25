using AutoMapper;
using LotterySpider.Common.Dtos.Lottery168;
using LotterySpider.Common.Models;

namespace LotterySpider
{
    public class AutoMapperConfig
    {
        public static void RegisteMaps()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Json168Sub, AggregateRoot>()
                    .ForMember(t => t.DrawTime, p => p.MapFrom(s => s.preDrawTime))
                    .ForMember(t => t.Issue, p => p.MapFrom(s => s.preDrawIssue))
                    .ForMember(t => t.preDrawCode, p => p.MapFrom(s => s.preDrawCode))
                    .ForMember(t => t.SingleOrDouble, p => p.MapFrom(s => s.sumSingleDouble))
                    .ForMember(t => t.SumNum, p => p.MapFrom(s => s.sumNum))
                    .Include<Json168SubQGC, Qgc>()
                    .Include<Json168SubJWC, Jwc>()
                    .Include<Json168SubJSC, Jsc>()
                    .Include<Json168SubRmc, Rmc>()
                    .Include<Json168SubRmc, Ssc>()
                    .Include<Json168SubShiyi5, Shiyi5>()
                    .Include<Json168SubK3, K3Model>();

                config.CreateMap<Json168SubQGC, Qgc>();
                config.CreateMap<Json168SubJWC, Jwc>();
                config.CreateMap<Json168SubJSC, Jsc>().ForMember(t => t.SumFS, p => p.MapFrom(s => s.sumFS));
                config.CreateMap<Json168SubRmc, Rmc>().ForMember(t => t.SumFS, p => p.MapFrom(s => s.sumFS));
                config.CreateMap<Json168SubRmc, Ssc>();

                config.CreateMap<Json168SubShiyi5, Shiyi5>()
                .ForMember(t => t.BehindThree, p => p.MapFrom(s => s.behindThree))
                .ForMember(t => t.BetweenThree, p => p.MapFrom(s => s.betweenThree))
                .ForMember(t => t.LastThree, p => p.MapFrom(s => s.lastThree));

                config.CreateMap<Json168SubK3, K3Model>()
                .ForMember(t => t.firstSeafood, p => p.MapFrom(s => s.firstSeafood))
                .ForMember(t => t.secondSeafood, p => p.MapFrom(s => s.secondSeafood))
                .ForMember(t => t.thirdSeafood, p => p.MapFrom(s => s.thirdSeafood))
                .ForMember(t => t.sumBigSmall, p => p.MapFrom(s => s.sumBigSmall));
            });
        }
    }
}