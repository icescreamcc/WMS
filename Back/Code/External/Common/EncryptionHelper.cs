using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace External.Common
{
    public class EncryptionHelper
    {

        #region MD5加密
        public static string MD5Encrypt(string str, bool is32 = true)
        {

            byte[] pwdByte = Encoding.UTF8.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            
            byte[] result = md5.ComputeHash(pwdByte);
            string strPwd = BitConverter.ToString(result);
            if (is32)
            {
                string[] p = strPwd.Split('-');
                string pw = "";
                foreach (string s in p)
                {
                    pw += s;
                }
                strPwd = pw.Trim();
            }
            return strPwd.ToLower();
        } 
        #endregion

        #region SHA256加密
        public static string SHA256Encrypt(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        } 
        #endregion

        #region Des加解密
        public static string EncryptionKey = ".net5.0_2021";

        public static string DesEncrypt(string str,string encryptionKey=null)
        {
            if (string.IsNullOrEmpty(encryptionKey))
            {
                encryptionKey = EncryptionKey;
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(encryptionKey.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(encryptionKey.Substring(0, 8));
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString(); 
        }

        public static string DesDecrypt(string str, string encryptionKey = null)
        {
            if (string.IsNullOrEmpty(encryptionKey))
            {
                encryptionKey = EncryptionKey;
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(encryptionKey.Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(encryptionKey.Substring(0, 8));
            byte[] buffer = new byte[str.Length / 2];
            for (int i = 0; i < (str.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            var res= Encoding.UTF8.GetString(stream.ToArray());
            return res;
        } 
        #endregion

        #region Base64加解密
        public static string Base64Encrypt(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes); 
        }

        public static string Base64Decrypt(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return Encoding.Default.GetString(bytes); 
        }

        public static bool TryBase64Encoded(string encryptStr, out string decryptStr)
        {
            try
            {
                var bytes = Convert.FromBase64String(encryptStr);
                decryptStr = Encoding.UTF8.GetString(bytes);
                return true;
            }
            catch (Exception)
            {
                decryptStr = string.Empty;
                return false;
            }
        }
        #endregion
    }
}
