using LotterySpider.Common.Utils;

namespace LotterySpider.Common.Models
{
    public class Shiyi5 : AggregateRoot
    {
        public string DragonTiger
        {
            get
            {
                return preDrawCode.GetDragonTiger(true);
            }
        }

        public int BehindThree { get; set; }
        public int BetweenThree { get; set; }
        public int LastThree { get; set; }
    }
}