using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BoService.Models;

namespace BoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private IConfiguration _config;
        public BoAppDB Db { get; }
        public UserLoginController(BoAppDB db, IConfiguration config)
        {
            Db = db;
            _config = config;
        }
        // POST api/blog
        [HttpPost]
        public Dictionary<string, object> Post([FromBody] BoService.Models.Users value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;
                string username = value.UserName;
                string password = value.Password;
                BoService.Models.Users objUsers = new BoService.Models.Users(Db);
               
                Users lstobjUsers = objUsers.GetUsersList(value.UserName, value.Password).FirstOrDefault();

                //bIsValidUser = objUsers.IsUserRegistered();
                if(lstobjUsers!=null)
                {
                    var jwt = new BoService.Authentication.JwtService(_config);
                    var token = jwt.GenerateSecurityToken(value.UserName);

                    if(lstobjUsers.Role.Contains("Member"))
                    {
                        response.Add("status", "success");
                        response.Add("User Status", "Valid User Credential...");
                        response.Add("User Token", token);

                    }


                }
                else
                {
                    response.Add("status", "Error");
                    response.Add("message", "Invalid User Credentials...");
                }
            }
            catch(Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("message", Ex.Message);
            }
            return response;
        }
     }
}
