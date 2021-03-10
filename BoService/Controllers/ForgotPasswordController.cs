using BoService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace BoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : Controller
    {

        private IConfiguration _config;

        public BoAppDB Db { get; }
        

        public ForgotPasswordController(BoAppDB db, IConfiguration config)
        {
            Db = db;
            _config = config;
           


        }

        static bool mailSent = false;


        [HttpPost]
        public async Task<Dictionary<string, object>> Post([FromBody] User value)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                Db.Connection.Open();
                bool bIsValidUser = false;

                BoService.Models.User objUsers = new BoService.Models.User(Db);

                User lstobjUsers = objUsers.GetUsersListByUserName(value.UserName).FirstOrDefault();

                //bIsValidUser = objUsers.IsUserRegistered();
                if (lstobjUsers != null)
                {
                    var jwt = new BoService.Authentication.JwtService(_config);
                    var token = jwt.GenerateSecurityToken(value.UserName);
                    //if(lstobjUsers.Role.Contains("Member"))
                    //{
                    response.Add("Status", "Success");
                    response.Add("Message", "Email Send Successfully!!");
                    response.Add("token", token);
                    response.Add("user", lstobjUsers);
                    // }

                    var param = new Dictionary<string, string>
            {
                {"token", token },
                {"username", value.UserName }
            };

                    
                }


                else
                {
                    response.Add("Status", "Error");
                    response.Add("Message", "Invalid User Name...");
                }
            }
            catch (Exception Ex)
            {
                response.Add("Status", "Error");
                response.Add("Message", Ex.Message);
            }
            return response;
        }
    }
}
