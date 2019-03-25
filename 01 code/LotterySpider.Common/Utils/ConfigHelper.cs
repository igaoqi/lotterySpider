using System;
using System.Configuration;

namespace LotterySpider.Common.Utils
{
    public class ConfigHelper
    {
        public static string GetString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object obj = CacheHelper.GetCache(cacheKey);
            if (obj == null)
            {
                try
                {
                    obj = ConfigurationManager.AppSettings[key];
                    if (obj != null)
                    {
                        CacheHelper.SetCache(cacheKey, obj, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return string.Concat(obj);
        }

        public static int GetInt(string key)
        {
            int.TryParse(ConfigurationManager.AppSettings[key], out int c);
            return c;
        }

        public static DateTime GetDate(string key)
        {
            DateTime dt = DateTime.Parse("2016-01-01");
            DateTime.TryParse(ConfigurationManager.AppSettings[key], out dt);
            return dt;
        }
    }
}