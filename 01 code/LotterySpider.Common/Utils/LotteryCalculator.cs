using LotterySpider.Common.Enums.KaiJiang168;

namespace LotterySpider.Common.Utils
{
    public class LotteryCalculator
    {
        /// <summary>
        /// 获取龙虎和值
        /// </summary>
        /// <param name="drawNums"></param>
        /// <param name="single">是否只取单个龙虎</param>
        /// <returns>exp:1,2,1</returns>
        public static string GetDragonTiger(string drawNums, bool single = false)
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