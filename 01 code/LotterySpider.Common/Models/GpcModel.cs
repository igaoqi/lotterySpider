using LotterySpider.Common.Utils;

namespace LotterySpider.Common.Models
{
    public class GpcModel : AggregateRoot
    {
        public string DragonTiger
        {
            get
            {
                return preDrawCode.GetDragonTiger();
            }
        }
    }
}