using LotterySpider.Common.Utils;

namespace LotterySpider.Core.DbContext
{
    public class DbFactory
    {
        public static string GetConnString(string stringKey = "MasterDb")
        {
            return ConfigHelper.GetString(stringKey);
        }
    }
}