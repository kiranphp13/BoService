using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BoService.Models;
using Microsoft.AspNetCore.Authorization;

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
        public Dictionary<string, object> Post([FromBody] BoService.Models.User value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;
                string username = value.UserName;
                string password = value.Password;
                BoService.Models.User objUsers = new BoService.Models.User(Db);

                User lstobjUsers = objUsers.GetUsersList(value.UserName, value.Password).FirstOrDefault();

                //bIsValidUser = objUsers.IsUserRegistered();
                if (lstobjUsers != null)
                {
                    var jwt = new BoService.Authentication.JwtService(_config);
                    var token = jwt.GenerateSecurityToken(value.Email);

                    //if(lstobjUsers.Role.Contains("Member"))
                    //{
                    response.Add("Status", "Success");
                    response.Add("Message", "Valid User Credential...");
                    response.Add("token", token);
                    response.Add("user", lstobjUsers);
                    // }


                }
                else
                {
                    response.Add("Status", "Error");
                    response.Add("Message", "Invalid User Credentials...");
                }
            }
            catch (Exception Ex)
            {
                response.Add("Status", "Error");
                response.Add("Message", Ex.Message);
            }
            return response;
        }



        //// POST api/blog
        //[HttpPost]
        //public Dictionary<string, object> Login(string email,string password)
        //{
        //    Dictionary<string, object> response = new Dictionary<string, object>();
        //    try
        //    {
        //        Db.Connection.Open();
        //        bool bIsValidUser = false;

        //        BoService.Models.User objUsers = new BoService.Models.User(Db);

        //        User lstobjUsers = objUsers.GetUsersList(email, password).FirstOrDefault();

        //        //bIsValidUser = objUsers.IsUserRegistered();
        //        if (lstobjUsers != null)
        //        {
        //            var jwt = new BoService.Authentication.JwtService(_config);
        //            var token = jwt.GenerateSecurityToken(email);

        //            if (lstobjUsers.Role.Contains("Member"))
        //            {
        //                response.Add("status", "success");
        //                response.Add("User Status", "Valid User Credential...");
        //                response.Add("User Token", token);

        //            }


        //        }
        //        else
        //        {
        //            response.Add("status", "Error");
        //            response.Add("message", "Invalid User Credentials...");
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        response.Add("status", "Error");
        //        response.Add("message", Ex.Message);
        //    }
        //    return response;
        //}
    }
}
