using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AggregationService.ProviderJWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitDLL;

namespace AggregationService.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]User user)
        {
            string userString = HttpContext.Session.GetString("Login");
            userString = userString != null ? userString : "";

            User userTruly = DefaultController.privateWeakCheckByPassword(user).Result;
            if (userTruly == null)
            {
                StatisticSender.SendStatistic("Token", DateTime.Now, "Create Token", Request.HttpContext.Connection.RemoteIpAddress.ToString(), false, userString);
                return Unauthorized();
            }
            else
            {
                var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject(userTruly.Login)
                                .AddIssuer("Test.Security.Bearer")
                                .AddAudience("Test.Security.Bearer")
                                .AddClaim(userTruly.Role, userTruly.ID.ToString())
                                .AddExpiry(200)
                                .Build();
                HttpContext.Session.SetString("Token", token.Value);
                HttpContext.Session.SetString("Login", user.Login);

                //пихаем новый токен пользователю в бд
                var values = new JObject();
                values.Add("id", userTruly.ID);
                values.Add("login", userTruly.Login);
                values.Add("password", userTruly.Password);
                values.Add("role", userTruly.Role);
                values.Add("lasttoken", token.Value);

                var result = await QueryClient.SendQueryToService(HttpMethod.Put, "http://localhost:54196", "/api/Users/" + userTruly.ID, null, values);
                try
                {
                    User resultUser = JsonConvert.DeserializeObject<User>(result);
                    StatisticSender.SendStatistic("Token", DateTime.Now, "Create Token", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, userString);
                    return Ok(resultUser);
                }
                catch
                {
                    return Unauthorized();
                }
            }
        }


        public static User CreateToken(User user)
        {
            User userTruly = DefaultController.privateWeakCheckByPassword(user).Result;
            if (userTruly == null)
            {
                return userTruly;
            }
            else
            {
                var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject(userTruly.Login)
                                .AddIssuer("Test.Security.Bearer")
                                .AddAudience("Test.Security.Bearer")
                                .AddClaim(userTruly.Role, userTruly.ID.ToString())
                                .AddExpiry(200)
                                .Build();

                //пихаем новый токен пользователю в бд
                var values = new JObject();
                values.Add("id", userTruly.ID);
                values.Add("login", userTruly.Login);
                values.Add("password", userTruly.Password);
                values.Add("role", userTruly.Role);
                values.Add("lasttoken", token.Value);

                var result = QueryClient.SendQueryToService(HttpMethod.Put, "http://localhost:54196", "/api/Users/" + userTruly.ID, null, values).Result;
                try
                {
                    User resultUser = JsonConvert.DeserializeObject<User>(result);
                    return resultUser;
                }
                catch
                {
                    return userTruly;
                }
            }
        }
    }
}