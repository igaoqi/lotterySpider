using LotterySpider.Common.Utils;

namespace LotterySpider.Common.Models
{
    /// <summary>
    /// 热门彩
    /// </summary>
    public class Rmc : AggregateRoot
    {
        public string DragonTiger
        {
            get
            {
                return preDrawCode.GetDragonTiger();
            }
        }

        public int SumFS { get; set; }
    }
}