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

namespace LotterySpider.Core.Jobs.Impls.JiSuCai
{
    /// <summary>
    /// Jsssc
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class JssscJob : IJobProvider<List<Jsc>>
    {
        public string CacheKey
        {
            get
            {
                return "Cachekey_Jsssc_{0}";
            }
        }

        public string Url
        {
            get
            {
                return ConfigHelper.GetString("168LotteryAPI") + "CQShiCai/getBaseCQShiCaiList.do?date={0}&lotCode={1}";
            }
        }

        public DateTime JobDate
        {
            get
            {
                string sql = "SELECT MAX(DRAWTIME) DATE FROM jsc_Jsssc LIMIT 1";
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
            Console.WriteLine("Jsssc" + DateTime.Now);

            DateTime stDate = JobDate;
            DateTime endDate = DateTime.Now.Date;
            for (; stDate <= endDate; stDate = stDate.AddDays(1))
            {
                var data = GetJson(stDate.ToCustomDateString());
                List<Jsc> models = Mapper.Map<List<Json168SubJSC>, List<Jsc>>(data);
                Insert(models);
            }
        }

        public void Insert(List<Jsc> models)
        {
            if (models == null || !models.Any())
                return;
            Filter(ref models);
            if (!models.Any())
                return;

            using (var conn = new MySqlConnection(DbFactory.GetConnString()))
            {
                string sql = @"INSERT INTO jsc_Jsssc(Issue,preDrawCode,DrawTime,DrawDate,SingleOrDouble,DragonTiger,SumNum) VALUES (@Issue,@preDrawCode,@DrawTime,@DrawDate,@SingleOrDouble,@DragonTiger,@SumNum)
                               ON DUPLICATE KEY UPDATE preDrawCode=@preDrawCode,DrawTime=@DrawTime,DrawDate=@DrawDate,SingleOrDouble=@SingleOrDouble,DragonTiger=@DragonTiger,SumNum=@SumNum";

                int maxInsertCount = ConfigHelper.GetInt("MaxInsertCount");
                for (int i = 0; i <= models.Count / maxInsertCount; i++)
                {
                    conn.Execute(sql, models.Skip(i * maxInsertCount).Take(maxInsertCount).ToList());
                }
            }
        }

        public void Filter(ref List<Jsc> items)
        {
            string key = null;
            List<Jsc> queue = new List<Jsc>();
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

        private List<Json168SubJSC> GetJson(string date)
        {
            var data = RequestHelper.Get<Json168Root<List<Json168SubJSC>>>(string.Format(Url, date, LotteryCode.JiSuCai.Jsssc));
            if (data == null || data.errorCode != 0 || data.result == null || !data.result.data.Any())
                return null;
            return data.result.data;
        }
    }
}