using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitDLL;

namespace AggregationService.Controllers
{
    [Produces("application/json")]
    [Route("api/Default")]
    public class DefaultController : Controller
    {
        public static async Task<User> privateWeakCheck(User user)
        {
            var values = new JObject();
            values.Add("Login", user.Login);
            values.Add("LastToken", user.LastToken);
            try
            {
                var result = await QueryClient.SendQueryToService(HttpMethod.Post, "http://localhost:54196", "/api/Users/Find", null, values);
                return JsonConvert.DeserializeObject<User>(result);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User> privateWeakCheckByPassword(User user)
        {
            var values = new JObject();
            values.Add("Login", user.Login);
            values.Add("Password", user.Password);

            try
            {
                var result = await QueryClient.SendQueryToService(HttpMethod.Post, "http://localhost:54196", "/api/Users/Find", null, values);
                return JsonConvert.DeserializeObject<User>(result);
            }
            catch
            {
                return null;
            }
        }
    }
}