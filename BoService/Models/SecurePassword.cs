using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace BoService.Models
{
    public static class SecurePassword
    {
        public enum EncDecType
        {
            BASE64 = 0,
            MD5 = 1,
            AES = 2
        }
        public static string EncryptPassword(string strPassword, EncDecType EncOrDecType)
        {
            string strReturnPassword = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strPassword))
                {
                    throw new ArgumentNullException("Password should not be null or empty.");
                }
                else
                {
                    switch (EncOrDecType)
                    {
                        case EncDecType.BASE64:
                            strReturnPassword = EncryptBase64Password(strPassword);
                            break;
                        case EncDecType.MD5:
                            strReturnPassword = EnclryptMD5Password(strPassword);
                            break;
                        case EncDecType.AES:
                            strReturnPassword = EncryptAESPassword(strPassword);
                            break;

                    }
                }
            }
            catch(Exception Ex)
            {

            }
            return strReturnPassword;
        }

        public static string DecryptPassword(string strPassword, EncDecType EncOrDecType)
        {
            string strReturnPassword = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strPassword))
                {
                    throw new ArgumentNullException("Password should not be null or empty.");
                }
                else
                {
                    switch (EncOrDecType)
                    {
                        case EncDecType.BASE64:
                            strReturnPassword = DecryptBase64Password(strPassword);
                            break;
                        case EncDecType.MD5:
                            strReturnPassword = DeclryptMD5Password(strPassword);
                            break;
                        case EncDecType.AES:
                            strReturnPassword = DecryptAESPassword(strPassword);
                            break;

                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

        private static string EncryptBase64Password(string strPassword)
        {
            string strReturnPassword = string.Empty;
            try
            {
                byte[] passwordByteArray = new byte[strPassword.Length];
                passwordByteArray = System.Text.Encoding.UTF8.GetBytes(strPassword);
                strReturnPassword = Convert.ToBase64String(passwordByteArray);
            }
            catch(Exception Ex)
            {

            }
            return strReturnPassword;
        }

        private static string GetMd5Hash(MD5 md5Hash, string password)
        {
            byte[] inputData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder hashBuilder = new StringBuilder();
            for (int i = 0; i < inputData.Length; i++)
            {
                hashBuilder.Append(inputData[i].ToString("x2"));
            }
            
            return hashBuilder.ToString();
        }

        private static string EnclryptMD5Password(string strPassword)
        {
            string strReturnPassword = string.Empty;
            try
            {
                byte[] data = UTF8Encoding.UTF8.GetBytes(strPassword);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    string strMD5Hash = GetMd5Hash(md5, strPassword);
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(strMD5Hash));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        strReturnPassword = Convert.ToBase64String(results, 0, results.Length);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

        private static string EncryptAESPassword(string strPassword)
        {
            string strReturnPassword = string.Empty;
            try
            {

            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

        private static string DecryptBase64Password(string strPassword)
        {
            string strReturnPassword = string.Empty;
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(strPassword);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                strReturnPassword = new String(decoded_char);
                
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

        private static string DeclryptMD5Password(string strPassword)
        {
            string strReturnPassword = string.Empty;
            try
            {
                byte[] data = Convert.FromBase64String(strPassword); // decrypt the incrypted text
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    string strMD5Hash = GetMd5Hash(md5, strPassword);
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(strMD5Hash));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        strReturnPassword = UTF8Encoding.UTF8.GetString(results);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

       

        private static string DecryptAESPassword(string strPassword)
        {
            string strReturnPassword = string.Empty;
            try
            {

            }
            catch (Exception Ex)
            {

            }
            return strReturnPassword;
        }

    }
}
