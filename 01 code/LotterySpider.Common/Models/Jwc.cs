using LotterySpider.Common.Utils;

namespace LotterySpider.Common.Models
{
    /// <summary>
    /// 境外彩
    /// </summary>
    public class Jwc : AggregateRoot
    {
        public string DragonTiger
        {
            get
            {
                return LotteryCalculator.GetDragonTiger(preDrawCode);
            }
        }
    }
}