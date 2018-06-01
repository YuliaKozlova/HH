using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AggregationService.Models;
using RabbitDLL;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace AggregationService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "Index", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            return View();
        }

        [Route("Users")]
        public IActionResult GetUsers()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            try
            {
                string result = QueryClient.SendQueryToService(HttpMethod.Get, RabbitDLL.Linker.Users, "/api/users", null, null).Result;
                List<User> objectToView = JsonConvert.DeserializeObject<List<User>>(result);
                StatisticSender.SendStatistic("Home", DateTime.Now, "GetUsers", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
                return View(objectToView);
            }
            catch
            {
                return View("Error", "Service of Users unavailable");
            }
        }

        [Route("Vacancys")]
        public IActionResult GetVacancys()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            string result = QueryClient.SendQueryToService(HttpMethod.Get, RabbitDLL.Linker.Vacancys, "/api/VacancyItems", null, null).Result;
            try
            {
                List<VacancyItems> objectToView = JsonConvert.DeserializeObject<List<VacancyItems>>(result);
                StatisticSender.SendStatistic("Home", DateTime.Now, "GetVacancys", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
                return View(objectToView);
            }
            catch
            {
                return View("Error", "Service of Vacancys Unavailable");
            }
        }

        [Route("Resumes")]
        public IActionResult GetResumes()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            try
            {
                string result = QueryClient.SendQueryToService(HttpMethod.Get, RabbitDLL.Linker.Resumes, "/api/Resumes", null, null).Result;
                List<Resume> objectToView = JsonConvert.DeserializeObject<List<Resume>>(result);
                StatisticSender.SendStatistic("Home", DateTime.Now, "GetVacancys", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
                return View(objectToView);
            }
            catch
            {
                return View("Error", "Service of Resumes unavailable");
            }
        }

        [Route("Resumes/Edite/{id?}")]
        public IActionResult EditeResumes(int? id)
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            try
            {
                string result = QueryClient.SendQueryToService(HttpMethod.Get, RabbitDLL.Linker.Resumes, "/api/Resumes/"+id, null, null).Result;
                Resume objectToView = JsonConvert.DeserializeObject<Resume>(result);
                StatisticSender.SendStatistic("Home", DateTime.Now, "EditeResumes", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
                return View(objectToView);
            }
            catch
            {
                return View("Error", "Service of Resumes unavailable");
            }
        }

        [HttpPost]
        [Route("Resumes/Edite/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditeResumes([Bind("ID,Speiality,ResumeName,Salary,Age")] Resume rsm)
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            var values = new JObject();
            values.Add("ID", rsm.ID);
            values.Add("Speiality", rsm.Speiality);
            values.Add("ResumeName", rsm.ResumeName);
            values.Add("Salary", rsm.Salary);
            values.Add("Age", rsm.Age);
            try
            {
                string result = QueryClient.SendQueryToService(HttpMethod.Put, RabbitDLL.Linker.Resumes, "/api/Resumes/" + rsm.ID, null, values).Result;
                Resume objectToView = JsonConvert.DeserializeObject<Resume>(result);
                StatisticSender.SendStatistic("Home", DateTime.Now, "EditeVacancys", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
                return View("Index");
            }
            catch
            {
                return View("Error", "Service of Resumes unavailable");
            }
        }

        [Route("AllItems")]
        [Authorize(Policy = "Admin")]
        public IActionResult AllItems()
        {
            try
            {
                string result = QueryClient.SendQueryToService(HttpMethod.Get, RabbitDLL.Linker.Resumes, "/api/Resumes/full/cortege", null, null).Result;
                FullView objectToView = JsonConvert.DeserializeObject<FullView>(result);
                string user = HttpContext.Session.GetString("Login");
                user = user != null ? user : "";
                StatisticSender.SendStatistic("Home", DateTime.Now, "All items", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
                return View(objectToView);
            }
            catch
            {
                return View("Error", "Service of Resumes unavailable");
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "About", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "Contacts", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            return View();
        }

        public IActionResult Error()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "Error", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("LogOut")]
        public IActionResult LogOut()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "LogOut", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            HttpContext.Session.SetString("Token", "");
            HttpContext.Session.SetString("Login", "");
            return RedirectToAction(nameof(Index));
        }

        [Route("Registration")]
        public IActionResult Registration()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "Registration Start", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            return View();
        }

        [Route("Registration")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Login, Password")] User user)
        {
            var values = new JObject();
            values.Add("Login", user.Login);
            values.Add("Password", user.Password);
            values.Add("Role", "User");

            string usr = HttpContext.Session.GetString("Login");
            usr = usr != null ? usr : "";

            try
            {
                var result = await QueryClient.SendQueryToService(HttpMethod.Post, "http://localhost:54196", "/api/Users", null, values);
                User resultUser = JsonConvert.DeserializeObject<User>(result);

                StatisticSender.SendStatistic("Home", DateTime.Now, "Registration", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, usr);
                return RedirectToAction("Authorisation");
            }
            catch
            {
                StatisticSender.SendStatistic("Home", DateTime.Now, "Registration", Request.HttpContext.Connection.RemoteIpAddress.ToString(), false, usr);
                return View("Error", "Cannot create this User. Try again later or input another Data");
            }
        }

        [Route("Authorisation")]
        public IActionResult Authorisation()
        {
            string user = HttpContext.Session.GetString("Login");
            user = user != null ? user : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "Authorisation Start", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user);
            return View();
        }

        [Route("Authorisation")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authoristation([Bind("Login, Password")] User user)
        {
            string user1 = HttpContext.Session.GetString("Login");
            user1 = user1 != null ? user1 : "";
            StatisticSender.SendStatistic("Home", DateTime.Now, "Authorisation Ends", Request.HttpContext.Connection.RemoteIpAddress.ToString(), true, user1);
            return await privateAuth(user);
        }

        public async Task<IActionResult> privateAuth(User realUser)
        {
            if(realUser != null)
            {
                realUser = TokenController.CreateToken(realUser);
                if (realUser != null)
                {
                    HttpContext.Session.SetString("Token", realUser.LastToken);
                    HttpContext.Session.SetString("Login", realUser.Login);
                    return View("Index");
                }
                else
                {
                    return View("Error", "User input incorrect or Service unavailable");
                }
            }
            else
            {
                return View("Error", "User input incorrect or Service unavailable");
            }
        }
    }
}
