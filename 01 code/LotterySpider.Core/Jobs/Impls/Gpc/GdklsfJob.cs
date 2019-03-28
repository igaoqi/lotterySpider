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

namespace LotterySpider.Core.Jobs.Impls.Gpc
{
    /// <summary>
    /// Cqxync 重庆幸运农场
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class GdklsfJob : IJobProvider<List<GpcModel>>
    {
        public string CacheKey
        {
            get
            {
                return "Cachekey_gpc_Gdklsf_{0}";
            }
        }

        public string Url
        {
            get
            {
                return ConfigHelper.GetString("168LotteryAPI") + "klsf/getHistoryLotteryInfo.do?date={0}&lotCode={1}";
            }
        }

        public DateTime JobDate
        {
            get
            {
                string sql = "SELECT MAX(DRAWTIME) DATE FROM gpc_Gdklsf LIMIT 1";
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
            Console.WriteLine(string.Format("{0} \t {1}", DateTime.Now, context.JobDetail.Description));

            DateTime stDate = JobDate;
            DateTime endDate = DateTime.Now.Date;
            for (; stDate <= endDate; stDate = stDate.AddDays(1))
            {
                var data = GetJson(stDate.ToCustomDateString());
                List<GpcModel> models = Mapper.Map<List<Json168SubGpc>, List<GpcModel>>(data);
                Insert(models);
            }
        }

        public void Insert(List<GpcModel> models)
        {
            if (models == null || !models.Any())
                return;
            Filter(ref models);
            if (!models.Any())
                return;

            using (var conn = new MySqlConnection(DbFactory.GetConnString()))
            {
                string sql = @"INSERT INTO gpc_Gdklsf (Issue,preDrawCode,DrawTime,DrawDate,SumNum,SingleOrDouble,DragonTiger)
                                        SELECT @Issue,@preDrawCode,@DrawTime,@DrawDate,@SumNum,@SingleOrDouble,@DragonTiger FROM DUAL
                                        WHERE NOT EXISTS(SELECT 1 FROM gpc_Gdklsf WHERE Issue =@Issue) ";

                int maxInsertCount = ConfigHelper.GetInt("MaxInsertCount");
                for (int i = 0; i <= models.Count / maxInsertCount; i++)
                {
                    conn.Execute(sql, models.Skip(i * maxInsertCount).Take(maxInsertCount).ToList());
                }
            }
        }

        public void Filter(ref List<GpcModel> items)
        {
            string key = null;
            List<GpcModel> queue = new List<GpcModel>();
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

        private List<Json168SubGpc> GetJson(string date)
        {
            var data = RequestHelper.Get<Json168Root<List<Json168SubGpc>>>(string.Format(Url, date, LotteryCode.Gpc.Gdklsf));
            if (data == null || data.errorCode != 0 || data.result == null || !data.result.data.Any())
                return null;
            return data.result.data;
        }
    }
}