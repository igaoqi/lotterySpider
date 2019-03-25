using LotterySpider.Common.Utils;

namespace LotterySpider.Common.Models
{
    /// <summary>
    /// 极速彩
    /// </summary>
    public class Jsc : AggregateRoot
    {
        public string DragonTiger
        {
            get
            {
                return LotteryCalculator.GetDragonTiger(preDrawCode);
            }
        }

        public int SumFS { get; set; }
    }
}