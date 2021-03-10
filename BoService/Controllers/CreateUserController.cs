using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoService.Models;
using Microsoft.Extensions.Configuration;

namespace BoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        public BoAppDB Db { get; }
        private IConfiguration _config;
        public CreateUserController(BoAppDB db, IConfiguration config)
        {
            Db = db;
            _config = config;
        }

        // POST api/blog
        [HttpPost]
        public Dictionary<string, object> Post([FromBody] BoService.Models.User value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;

                BoService.Models.User objUser = new BoService.Models.User(Db);

                bool bIsUserCreated = false;
                objUser.UserName = value.UserName;
                objUser.Password = value.Password;

                bIsValidUser = objUser.IsUserRegistered();
                if (bIsValidUser == false)
                {
                    bIsUserCreated = objUser.CreateUser(value);
                    if (bIsUserCreated == true)
                    {
                        var jwt = new BoService.Authentication.JwtService(_config);
                        var token = jwt.GenerateSecurityToken(value.UserName);

                        response.Add("Status", "Success");
                        response.Add("Message", "User is added successfully...");
                        response.Add("token", token);
                        response.Add("user", objUser);
                    }
                    else
                    {
                        response.Add("Status", "Error");
                        response.Add("Message", "There is a problem in adding user...");
                    }
                }
            }
            catch (Exception Ex)
            {
                response.Add("status", "Error");
                response.Add("Message", Ex.Message);
            }
            return response;
        }
    }
}