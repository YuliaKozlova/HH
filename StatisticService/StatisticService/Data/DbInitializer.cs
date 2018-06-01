using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticService.Data
{
    public class DbInitializer
    {
        public static void Initialize(StatContext context)
        {
            context.Database.EnsureCreated();

            if (context.Statistic.Any())
            {
                return;   // DB has been seeded
            }

            var statistic = new RabbitDLL.Statistic[]
            {
                new RabbitDLL.Statistic() { Action = "Index", Client = "localhost", Result = true, PageName = "AggregationService", TimeStamp = DateTime.Now, User = "sad" }
            };
            foreach (RabbitDLL.Statistic s in statistic)
            {
                context.Statistic.Add(s);
            }
            context.SaveChanges();

            var statisticQueue = new RabbitDLL.Statistic[]
            {
                new RabbitDLL.Statistic() { Action = "Index", Client = "localhost", Result = true, PageName = "AggregationService", TimeStamp = DateTime.Now, User = "sad" }
            };
            foreach (RabbitDLL.Statistic s in statisticQueue)
            {
                context.StatisticFromQueue.Add(s);
            }

            context.SaveChanges();
        }
    }
}