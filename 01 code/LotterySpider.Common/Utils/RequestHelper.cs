using RestSharp;
using System.Threading;

namespace LotterySpider.Common.Utils
{
    public class RequestHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="waitSecond">开奖动画等待时间</param>
        /// <returns></returns>
        public static T Get<T>(string url, int waitSecond = 5) where T : new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            for (int i = 0; i <= ConfigHelper.GetInt("RetryCount"); i++)
            {
                Thread.Sleep(waitSecond * 1000);
                var data = client.Execute<T>(request);
                if (data.StatusCode != System.Net.HttpStatusCode.OK)
                    continue;
                if (string.IsNullOrEmpty(data.Content))
                    continue;
                return data.Data;
            }
            return default(T);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <param name="waitSecond">开奖动画等待时间</param>
        /// <returns></returns>
        public static string Get(string url, int waitSecond = 5)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            for (int i = 0; i <= ConfigHelper.GetInt("RetryCount"); i++)
            {
                Thread.Sleep(waitSecond * 1000);
                var data = client.Execute(request);
                if (data.StatusCode != System.Net.HttpStatusCode.OK)
                    continue;
                if (string.IsNullOrEmpty(data.Content))
                    continue;
                return data.Content;
            }
            return default(string);
        }
    }
}