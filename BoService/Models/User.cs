using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoService.Models
{
    public class User
    {
        public string FullName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public string UserName { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public string Status { get; set; }
        public int Id { get; set; }

        internal BoAppDB Db { get; set; }

        public User()
        {
            FullName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;

        }

        internal User(BoAppDB db)
        {
            Db = db;
            FullName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Status = string.Empty;
        }

        public List<User> GetUsersList(string strUserName, string strUserOassword)
        {
            List<User> returnList = new List<User>();
            try
            {
                string strEncryptedPassword = EncryptPassword(strUserOassword);
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "SELECT * FROM edc.bousers where username = " + "'" + strUserName + "'" + "and password = " + "'" + strEncryptedPassword + "'";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }
        public List<User> GetUsersListByUserName(string strUserName)
        {
            List<User> returnList = new List<User>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "SELECT * FROM edc.bousers where username = " + "'" + strUserName + "'";
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }
        public List<User> GetUsersListByUserNameEdit(string strUserName, int userId)
        {
            List<User> returnList = new List<User>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "SELECT * FROM edc.bousers where username = " + "'" + strUserName + "' and id <>" + userId;
                cmd.CommandText = commandText;
                returnList = ReadAllAsync(cmd.ExecuteReader());
            }
            catch (Exception Ex)
            {
                returnList = null;
            }
            return returnList;
        }

        private List<User> ReadAllAsync(System.Data.Common.DbDataReader reader)
        {
            List<User> posts = new List<User>();
            try
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        var post = new User(Db)
                        {
                            FullName = reader.GetString(0),
                            Password = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Role = reader.GetString(4),
                        };
                        posts.Add(post);
                    }
                }
            }
            catch (Exception Ex)
            {
                posts = null;
            }
            return posts;
        }

        public bool IsUserNameExists()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<User> result = GetUsersListByUserName(UserName);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch (Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }
        public bool IsUserNameExistsEdit()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<User> result = GetUsersListByUserNameEdit(UserName, Id);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch (Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }
        public bool IsUserRegistered()
        {
            bool bIsUserRegistered = false;
            try
            {
                List<User> result = GetUsersList(UserName, Password);
                if (result != null && result.Count > 0)
                {
                    bIsUserRegistered = true;

                }
                else
                {
                    bIsUserRegistered = false;
                }
            }
            catch (Exception Ex)
            {
                bIsUserRegistered = false;
            }
            return bIsUserRegistered;
        }

        public bool CreateUser(User userData)
        {
            bool bIsRecordAdded = false;
            try
            {

                string Query = "insert into edc.bousers(FullName,UserName,Password,Email,Phone,Role,Status) values('" + userData.UserName + "','" + userData.FullName + "','" + userData.Password + "','" + userData.Email + "','" + userData.Phone + "','" + userData.Role + "','" + userData.Status + "');";
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                bIsRecordAdded = true;
            }
            catch (Exception ex)
            {
                bIsRecordAdded = false;
            }
            return bIsRecordAdded;
        }

        public bool UpdateUser(User userData)
        {
            bool bIsRecordAdded = false;
            try
            {
                string strEncryptedPassword = EncryptPassword(userData.Password);
                string Query = "update edc.bousers set FullName='" + userData.FullName + "',UserName='" + userData.UserName + "',Password='" + strEncryptedPassword + "',Email='" + userData.Email + "'," +
                    "Phone='" + userData.Phone + "',Role='" + userData.Role + "',Status='" + userData.Status + "',Address='" + userData.Address + "' where id=" + userData.Id + ";";
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = Query;
                cmd.ExecuteNonQuery();
                bIsRecordAdded = true;
            }
            catch (Exception ex)
            {
                bIsRecordAdded = false;
            }
            return bIsRecordAdded;
        }

        public string EncryptPassword(string strPassword)
        {
            string strReturnPassword = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(strPassword))
                {
                    throw new ArgumentNullException("Password should not be null or empty...");
                }
                else
                {
                    strReturnPassword = SecurePassword.EncryptPassword(strPassword, SecurePassword.EncDecType.BASE64);
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

        public string DecryptPassword(string strPassword)
        {
            string strReturnPassword = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(strPassword))
                {
                    throw new ArgumentNullException("Password should not be null or empty...");
                }
                else
                {
                    strReturnPassword = SecurePassword.DecryptPassword(strPassword, SecurePassword.EncDecType.BASE64);
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }
    }
}
