using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpHelper
{
    public static class MD5Helper
    {
        /// <summary>
        /// Will Encrypt a string to MD5 hash
        /// </summary>
        /// <param name="str">string to encrypt</param>
        /// <returns>The hash of a encrypted string</returns>
        public static string Md5Encypt(this string str)
        {
            string hash;
            using (MD5 md5 = MD5.Create())
                hash = GetMd5Hash(md5, str);
            return hash;
        }


        /// <summary>
        /// Will validate if a string is a valid MD5 hash
        /// </summary>
        /// <param name="str">string to validate</param>
        /// <returns>A boolean, saying if is valid or not</returns>
        public static bool IsValidMd5(this string str)
        {
            return string.IsNullOrEmpty(str) ? false : Regex.IsMatch(str, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled);
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(i.ToString("x2"));

            return sBuilder.ToString();
        }
    }
}
