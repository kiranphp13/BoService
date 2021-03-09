using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoService.Models
{
    public class Users
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

           
        public string Role { get; set; }

        public int ID { get; set; }

        internal BoAppDB Db { get; set; }

        public Users()
        {
            Name = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Role = string.Empty;
            ID = 0;
        }

        internal Users(BoAppDB db)
        {
            Db = db;
            Name = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }

        public List<Users> GetUsersList(string strEMail, string strUserOassword)
        {
            List<Users> returnList = new List<Users>();
            try
            {
                using var cmd = Db.Connection.CreateCommand();
                string commandText = "SELECT * FROM edc.bousers where Email = " + "'" + strEMail + "'" + "and password = " + "'" + strUserOassword + "'";
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
                List<Users> result = GetUsersList(Email, Password);
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

        public bool CreateUser(Users userData)
        {
            bool bIsRecordAdded = false;
            try
            {
                string strEncryptedPassword = EncryptPassword(userData.Password);
                //string strDecryptedPassword = DecryptPassword(strEncryptedPassword);
                string Query = "insert into edc.bousers(Name,Password,Email,Phone,Role,ID) values('" + userData.Name + "','" + strEncryptedPassword + "','" + userData.Email + "','" + userData.Phone + "','" + userData.Role + "','" + userData.ID + "');";
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
            catch(Exception Ex)
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
