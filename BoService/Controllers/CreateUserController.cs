using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        public BoAppDB Db { get; }

        public CreateUserController(BoAppDB db)
        {
            Db = db;
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

                BoService.Models.Users objUser = new BoService.Models.Users(Db);

                bool bIsUserCreated= false;

                
                bIsValidUser = objUser.IsUserRegistered();
                if(bIsValidUser == false)
                {
                    bIsUserCreated = objUser.CreateUser(value);
                    if(bIsUserCreated == true)
                    {
                        response.Add("Status", "success");
                        response.Add("User Created", "User is added successfully...");
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
