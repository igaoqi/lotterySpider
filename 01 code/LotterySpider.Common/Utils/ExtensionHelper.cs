using LotterySpider.Common.Enums.KaiJiang168;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotterySpider.Common.Utils
{
    public static class ExtensionHelper
    {
        public static string GetDragonTiger(this string drawNums, bool single = false)
        {
            if (string.IsNullOrEmpty(drawNums))
                return string.Empty;
            var nums = drawNums.Split(',');
            string result = string.Empty;
            for (int i = 0; i < nums.Length / 2; i++)
            {
                int first = int.Parse(nums[i]);
                int last = int.Parse(nums[nums.Length - i - 1]);

                if (first > last)
                    result += (int)DragonTiger.龙;
                else if (first < last)
                    result += (int)DragonTiger.虎;
                else
                    result += (int)DragonTiger.和;

                result += ",";

                if (single)
                    break;
            }
            return result.TrimEnd(',');
        }
    }
}