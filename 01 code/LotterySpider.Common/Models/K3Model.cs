namespace LotterySpider.Common.Models
{
    public class K3Model : AggregateRoot
    {
        public int sumBigSmall { get; set; }

        public int firstSeafood { get; set; }

        public int secondSeafood { get; set; }

        public int thirdSeafood { get; set; }
    }
}