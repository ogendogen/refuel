using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public static class Utils
    {
        public static string GenerateRandomString(int size)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*()_+{}|:\"<>?`-=[]\\;',./";

            int length = chars.Length;
            StringBuilder salt = new StringBuilder();
            for (int i=0; i<size; i++)
            {
                int choosen = RNGCryptoServiceProvider.GetInt32(length);
                salt.Append(chars[choosen]);
            }

            return salt.ToString();
        }
    }
}
