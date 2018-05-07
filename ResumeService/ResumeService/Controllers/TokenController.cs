using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resumeservice.Provider.JWT;

namespace Resumeservice.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        [Route("GetToken")]
        [HttpGet]
        public string GetToken()
        {
            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("Test-secret-key-1234"))
                                .AddSubject("admin")
                                .AddIssuer("Test.Security.Bearer")
                                .AddAudience("Test.Security.Bearer")
                                .AddClaim("admin", "1")
                                .AddExpiry(200)
                                .Build();
            return token.Value;
        }
    }
}