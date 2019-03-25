using AutoMapper;
using Dapper;
using LotterySpider.Core.DbContext;
using LotterySpider.Common.Models;
using LotterySpider.Common.Dtos.Lottery168;
using LotterySpider.Common.Utils;
using MySql.Data.MySqlClient;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LotterySpider.Core.Jobs.Impls.QuanGuoCai
{
    /// <summary>
    /// Pl5Job
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class Pl5Job : IJobProvider<List<Qgc>>
    {
        public string CacheKey
        {
            get
            {
                return "Cachekey_Pl5_{0}";
            }
        }

        public string Url
        {
            get
            {
                return ConfigHelper.GetString("168LotteryAPI") + "QuanGuoCai/getHistoryLotteryInfo.do?date={0}&lotCode={1}";
            }
        }

        public DateTime JobDate
        {
            get
            {
                string sql = "SELECT MAX(DRAWTIME) DATE FROM qgc_Pl5 LIMIT 1";
                using (var conn = new MySqlConnection(DbFactory.GetConnString()))
                {
                    var date = conn.ExecuteScalar<DateTime?>(sql);
                    if (date == null)
                        date = ConfigHelper.GetDate("DefaultDate");
                    return date.Value.Date;
                }
            }
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Pl5" + DateTime.Now);

            DateTime stDate = JobDate;
            DateTime endDate = DateTime.Now.Date;
            for (; stDate <= endDate; stDate = stDate.AddMonths(1))
            {
                var data = GetJson(stDate.ToCustomDateString());
                List<Qgc> models = Mapper.Map<List<Json168SubQGC>, List<Qgc>>(data);
                Insert(models);
            }
        }

        public void Insert(List<Qgc> models)
        {
            if (models == null || !models.Any())
                return;
            Filter(ref models);
            if (!models.Any())
                return;

            using (var conn = new MySqlConnection(DbFactory.GetConnString()))
            {
                string sql = @"INSERT INTO qgc_Pl5(Issue,preDrawCode,DrawTime,DrawDate,SumNum,SingleOrDouble) VALUES (@Issue,@preDrawCode,@DrawTime,@DrawDate,@SumNum,@SingleOrDouble)
                               ON DUPLICATE KEY UPDATE preDrawCode=@preDrawCode,DrawTime=@DrawTime,DrawDate=@DrawDate,SumNum=@SumNum,SingleOrDouble=@SingleOrDouble";

                int maxInsertCount = ConfigHelper.GetInt("MaxInsertCount");
                for (int i = 0; i <= models.Count / maxInsertCount; i++)
                {
                    conn.Execute(sql, models.Skip(i * maxInsertCount).Take(maxInsertCount).ToList());
                }
            }
        }

        public void Filter(ref List<Qgc> items)
        {
            string key = null;
            List<Qgc> queue = new List<Qgc>();
            foreach (var item in items)
            {
                key = string.Format(CacheKey, item.Issue);
                if (CacheHelper.GetCache(key) != null)
                    continue;
                CacheHelper.SetCache(key, item.preDrawCode, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                queue.Add(item);
            }
            items = queue;
        }

        private List<Json168SubQGC> GetJson(string date)
        {
            var data = RequestHelper.Get<Json168Root<List<Json168SubQGC>>>(string.Format(Url, date, LotteryCode.QuanGuoCai.Pl5));
            if (data == null || data.errorCode != 0 || data.result == null || !data.result.data.Any())
                return null;
            return data.result.data;
        }
    }
}