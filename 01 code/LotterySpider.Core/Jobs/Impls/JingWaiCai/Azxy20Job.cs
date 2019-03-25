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

namespace LotterySpider.Core.Jobs.Impls.JingWaiCai
{
    /// <summary>
    /// Azxy20 job
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class Azxy20Job : IJobProvider<List<Jwc>>
    {
        public string CacheKey
        {
            get
            {
                return "Cachekey_Azxy20_{0}";
            }
        }

        public string Url
        {
            get
            {
                return ConfigHelper.GetString("168LotteryAPI") + "LuckTwenty/getBaseLuckTwentyList.do?date={0}&lotCode={1}";
            }
        }

        public DateTime JobDate
        {
            get
            {
                string sql = "SELECT MAX(DRAWTIME) DATE FROM jwc_Azxy20 LIMIT 1";
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
            Console.WriteLine("Azxy20" + DateTime.Now);

            DateTime stDate = JobDate;
            DateTime endDate = DateTime.Now.Date;
            for (; stDate <= endDate; stDate = stDate.AddDays(1))
            {
                var data = GetData(stDate.ToCustomDateString());
                List<Jwc> models = Mapper.Map<List<Json168SubJWC>, List<Jwc>>(data);
                Insert(models);
            }
        }

        public void Insert(List<Jwc> models)
        {
            Filter(ref models);

            if (!models.Any())
                return;
            using (var conn = new MySqlConnection(DbFactory.GetConnString()))
            {
                string sql = @"INSERT INTO jwc_Azxy20(Issue,preDrawCode,DrawTime,DrawDate,SumNum,SingleOrDouble,DragonTiger) VALUES (@Issue,@preDrawCode,@DrawTime,@DrawDate,@SumNum,@SingleOrDouble,@DragonTiger)
                               ON DUPLICATE KEY UPDATE preDrawCode=@preDrawCode,DrawTime=@DrawTime,DrawDate=@DrawDate,SumNum=@SumNum,SingleOrDouble=@SingleOrDouble,DragonTiger=@DragonTiger";

                int maxInsertCount = ConfigHelper.GetInt("MaxInsertCount");
                for (int i = 0; i <= models.Count / maxInsertCount; i++)
                {
                    conn.Execute(sql, models.Skip(i * maxInsertCount).Take(maxInsertCount).ToList());
                }
            }
        }

        public void Filter(ref List<Jwc> items)
        {
            if (items == null || !items.Any())
                return;

            string key = null;
            List<Jwc> queue = new List<Jwc>();
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

        private List<Json168SubJWC> GetData(string date)
        {
            var data = RequestHelper.Get<Json168Root<List<Json168SubJWC>>>(string.Format(Url, date, LotteryCode.JingWaiCai.Azxy20), 10);
            if (data == null || data.errorCode != 0 || data.result == null || !data.result.data.Any())
                return null;
            return data.result.data;
        }
    }
}