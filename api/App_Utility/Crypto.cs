using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace api.App_Utility
{
    public class Crypto
    {
        public static string Encryption(string input)
        {
            string result = string.Empty;
            using (MD5 hash = MD5.Create())
            {
                byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));

                    result = sb.ToString();
                }
                return result;
            }
        }
    }
}