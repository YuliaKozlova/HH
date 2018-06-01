using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RabbitDLL
{
    public class Statistic
    {
        public int ID { get; set; }
        public string PageName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Action { get; set; }
        public string Client { get; set; }
        public bool Result { get; set; }
        public string User { get; set; }
    }

    public static class StatisticSender
    {
        public static void SendStatistic(string serviceName, DateTime dt, string action, string client, bool result, string user)
        {
            Statistic rbt = new Statistic() { PageName = serviceName, TimeStamp = dt, Action = action, Client = client, Result = result, User = user };
            var bus = RabbitHutch.CreateBus("host=localhost");
            var message = rbt;

            //отправляем его в очередь
            //bus.Publish(message);
            bus.Send("statistic", message);
        }
    }
}
