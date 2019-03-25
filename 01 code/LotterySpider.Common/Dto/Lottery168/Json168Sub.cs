using System;

namespace LotterySpider.Common.Dtos.Lottery168
{
    public class Json168Sub
    {
        public string preDrawCode { get; set; }
        public DateTime preDrawTime { get; set; }
        public long preDrawIssue { get; set; }
        public bool sumSingleDouble { get; set; }
        public int sumNum { get; set; }
    }
}