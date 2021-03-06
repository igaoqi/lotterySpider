﻿using AutoMapper;
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

namespace LotterySpider.Core.Jobs.Impls.Shiyix5
{
    /// <summary>
    /// Shiyi5_Jx
    /// </summary>
    [DisallowConcurrentExecution]
    public sealed class Shiyi5JxJob : IJobProvider<List<Shiyi5>>
    {
        public string CacheKey
        {
            get
            {
                return "Cachekey_Shiyi5_Jx_{0}";
            }
        }

        public string Url
        {
            get
            {
                return ConfigHelper.GetString("168LotteryAPI") + "ElevenFive/getElevenFiveList.do?date={0}&lotCode={1}";
            }
        }

        public DateTime JobDate
        {
            get
            {
                string sql = "SELECT MAX(DRAWTIME) DATE FROM Shiyi5_Jx LIMIT 1";
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
                List<Shiyi5> models = Mapper.Map<List<Json168SubShiyi5>, List<Shiyi5>>(data);
                Insert(models);
            }
        }

        public void Insert(List<Shiyi5> models)
        {
            if (models == null || !models.Any())
                return;
            Filter(ref models);
            if (!models.Any())
                return;

            using (var conn = new MySqlConnection(DbFactory.GetConnString()))
            {
                string sql = @"INSERT INTO Shiyi5_Jx(Issue,preDrawCode,DrawTime,DrawDate,SingleOrDouble,DragonTiger,SumNum,BehindThree,BetweenThree,LastThree) VALUES (@Issue,@preDrawCode,@DrawTime,@DrawDate,@SingleOrDouble,@DragonTiger,@SumNum,@BehindThree,@BetweenThree,@LastThree)
                               ON DUPLICATE KEY UPDATE preDrawCode=@preDrawCode,DrawTime=@DrawTime,DrawDate=@DrawDate,SingleOrDouble=@SingleOrDouble,DragonTiger=@DragonTiger,SumNum=@SumNum";

                int maxInsertCount = ConfigHelper.GetInt("MaxInsertCount");
                for (int i = 0; i <= models.Count / maxInsertCount; i++)
                {
                    conn.Execute(sql, models.Skip(i * maxInsertCount).Take(maxInsertCount).ToList());
                }
            }
        }

        public void Filter(ref List<Shiyi5> items)
        {
            string key = null;
            List<Shiyi5> queue = new List<Shiyi5>();
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

        private List<Json168SubShiyi5> GetJson(string date)
        {
            var data = RequestHelper.Get<Json168Root<List<Json168SubShiyi5>>>(string.Format(Url, date, LotteryCode.Shiyi5.Jx));
            if (data == null || data.errorCode != 0 || data.result == null || !data.result.data.Any())
                return null;
            return data.result.data;
        }
    }
}