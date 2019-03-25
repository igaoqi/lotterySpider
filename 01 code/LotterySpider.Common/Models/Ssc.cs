using LotterySpider.Common.Utils;

namespace LotterySpider.Common.Models
{
    /// <summary>
    /// 时时彩
    /// </summary>
    public class Ssc : AggregateRoot
    {
        public string DragonTiger
        {
            get
            {
                return preDrawCode.GetDragonTiger(true);
            }
        }
    }
}