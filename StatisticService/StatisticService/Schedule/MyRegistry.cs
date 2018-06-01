using EasyNetQ;
using FluentScheduler;
using RabbitDLL;
using StatisticService.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StatisticService.Schedule
{
    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            Schedule(() => ScheduleAction()).ToRunNow().AndEvery(3).Minutes();
        }

        private void ScheduleAction()
        {
            var Bus = RabbitHutch.CreateBus("host=localhost");
            ConcurrentStack<Statistic> statisticCollection = new ConcurrentStack<Statistic>();

            Bus.Receive<Statistic>("statistic", msg =>
            {
                Statistic stat = new Statistic() { ID = msg.ID, Client = msg.Client, Result = msg.Result, Action = msg.Action, PageName = msg.PageName, TimeStamp = msg.TimeStamp, User = msg.User };
                statisticCollection.Push(stat);

            });
            Thread.Sleep(15000);

            string connection = "Server=(localdb)\\mssqllocaldb;Database=Statistic99;Trusted_Connection=True;MultipleActiveResultSets=true";

            foreach (Statistic a in statisticCollection)
            {
                //пишем в таблицу StatisticFromQueue
                EventDbSender(a, connection);

                //сообщение в очередь обработанных. оттуда забираем аггрегацией и удаляем из таблицы висящих
                Statistic rbt = new Statistic() { PageName = a.PageName, TimeStamp = a.TimeStamp, Action = a.Action, Client = a.Client, Result = a.Result, User = a.User, ID = a.ID };
                var bus = RabbitHutch.CreateBus("host=localhost");
                var message = rbt;
                bus.Send("statisticRecieve", rbt);
            }
            Bus.Dispose();
        }

        private static void EventDbSender(Statistic rs, string connectionStringDb)
        {
            //_context.Statistic.Add(rs);
            //_context.SaveChanges();
            string connectionString = connectionStringDb;
            string query = string.Format("INSERT INTO StatisticFromQueue (Action, Client, PageName, Result, TimeStamp, [User]) " +
                    "VALUES (@Action, @Client, @PageName, @Result, @TimeStamp, @User)");

            // create connection and command
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("Action", SqlDbType.NVarChar).Value = rs.Action;
                cmd.Parameters.Add("Client", SqlDbType.NVarChar).Value = rs.Client;
                cmd.Parameters.Add("PageName", SqlDbType.NVarChar).Value = rs.PageName;
                cmd.Parameters.Add("Result", SqlDbType.Bit).Value = rs.Result;
                cmd.Parameters.Add("TimeStamp", SqlDbType.DateTime2).Value = rs.TimeStamp;
                if (rs.User != null)
                    cmd.Parameters.Add("User", SqlDbType.NVarChar).Value = rs.User;

                // open connection, execute INSERT, close connection
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}