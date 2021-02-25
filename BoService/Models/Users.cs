using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoService.Models
{
    public class Users
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }


        public string UserName { get; set; }
       
        public string Role { get; set; }

        internal BoAppDB Db { get; set; }

        public Users()
        {
            Name = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;

        }

        internal Users(BoAppDB db)
        {
            Db = db;
            Name = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }

        public List<Users> GetUsersList(string strUserName, string strUserOassword)
        {
            List<Users> returnList = new List<Users>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "SELECT * FROM edc.bousers where name = " + "'" + strUserName + "'" + "and password = " + "'" + strUserOassword + "'";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }

        private List<Users> ReadAllAsync(System.Data.Common.DbDataReader reader)
        {
            List<Users> posts = new List<Users>();
            try
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        var post = new Users(Db)
                        {
                            Name = reader.GetString(0),
                            Password = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Role = reader.GetString(4),
                        };
                        posts.Add(post);
                    }
                }
            }
            catch(Exception Ex)
            {
                posts = null;
            }
            return posts;
        }

        public bool IsUserRegistered()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<Users> result = GetUsersList(Name, Password);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch(Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }
    }
}
