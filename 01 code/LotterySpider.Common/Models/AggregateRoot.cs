using System;
using System.ComponentModel.DataAnnotations;

namespace LotterySpider.Common.Models
{
    public class AggregateRoot
    {
        [Key]
        public long Issue { get; set; }

        public string preDrawCode { get; set; }
        public DateTime DrawTime { get; set; }

        public DateTime DrawDate
        {
            get
            {
                return DrawTime.Date;
            }
        }

        public int SumNum { get; set; }

        public bool SingleOrDouble { get; set; }

        public string Remark { get; set; }
    }
}