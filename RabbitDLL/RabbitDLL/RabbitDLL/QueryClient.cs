using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AspNetCore.Http.Extensions;



namespace RabbitDLL
{
    public class QueryClient
    {
        public static async Task<string> SendQueryToService(HttpMethod method ,string ServiceUrl, string extraUrl, string user, JObject values)
        {
            //string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";

            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            string request = null;
            byte[] responseMessage;

            using (var client = new HttpClient())
            {
                //var token = new JwtTokenBuilder()
                //                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                //                .AddSubject("admin")
                //                .AddIssuer("Test.Security.Bearer")
                //                .AddAudience("Test.Security.Bearer")
                //                .AddClaim("admin", "1")
                //                .AddExpiry(200)
                //                .Build();

                client.BaseAddress = new Uri(ServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = null;
                if (method == HttpMethod.Get)
                {
                    response = await client.GetAsync(extraUrl);
                    request = "SERVICE: ArtistService \r\nGET: " + ServiceUrl + "/" + extraUrl + "\r\n" + client.DefaultRequestHeaders.ToString();
                }
                if (method == HttpMethod.Post)
                {
                    response = await client.PostAsJsonAsync(extraUrl, values);
                    request = "SERVICE: AuthorisationService \r\nPOST: " + ServiceUrl + "/" + extraUrl + "\r\n" + client.DefaultRequestHeaders.ToString() + "\r\n" + values;
                }
                if (method == HttpMethod.Put)
                {
                    
                    response = await client.PutAsJsonAsync(extraUrl, values);
                    request = "SERVICE: AuthorisationService \r\nPUT: " + ServiceUrl + "/" + extraUrl + "\r\n" + client.DefaultRequestHeaders.ToString() + "\r\n" + values;
                }
                if (method == HttpMethod.Delete)
                {
                    response = await client.DeleteAsync(extraUrl);
                    request = "SERVICE: AuthorisationService \r\nDelete: " + ServiceUrl + "/" + extraUrl + "\r\n" + client.DefaultRequestHeaders.ToString();
                }

                string responseString = response.Headers.ToString() + "\nStatus: " + response.StatusCode.ToString();

                if (response.IsSuccessStatusCode)
                {
                    responseMessage = await response.Content.ReadAsByteArrayAsync();
                    var jsonResult = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                }

                await Logger.LogQuery(request, responseString, responseMessage);
                return Encoding.UTF8.GetString(responseMessage); ;
            }
        }
    }
}
