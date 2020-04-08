using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ANZHFR.Core.Helpers
{
    public class StringHelper
    {
        public static string CreatePassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789-";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }

            return new string(chars);
        }

        //public static string CreateHash(string password)
        //{
        //    string salt = "DynamicSolversBangladesh";
        //    string passwrodSalt = String.Concat(password, salt);
        //    string hashedPwd = "";FormsAuthentication.HashPasswordForStoringInConfigFile(passwrodSalt, "sha1");
        //    return hashedPwd;
        //}

        public static string GenerateSaltedSHA1(string plainTextString)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(plainTextString));
            string sh1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sh1 += data[i].ToString("x2").ToUpperInvariant();
            }

            return sh1;
            
        }

    }
}
