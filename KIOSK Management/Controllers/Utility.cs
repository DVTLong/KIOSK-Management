using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KIOSK_Management.Controllers
{
    public class Utility
    {
        public static bool StringIsInvalid(string s)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
            {
                return true;
            }
            return false;
        }

        public static bool StringIsNumber(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool StringIsCMND(string s)
        {
            var regex = new Regex(@"^(\d{8}|\d{12})$");
            return regex.IsMatch(s);
        }

        public static bool StringIsEmail(string s)
        {            
            var regex = new Regex(@"\w+@\w+\.\w+");
            return regex.IsMatch(s);
        }

        public static bool StringIsPhoneNumber(string s)
        {
            string pattern = "";
            string[] dauSoDiDong = { "070", "079", "077", "076", "078", "089", "090", "093", "083", "084", "085", "081", "082", "088", "091", "094", "032", "033", "034", "035", "036", "037", "038", "039", "086", "096", "097", "098", "056", "058", "092", "059", "099" };
            for (int i=0; i < dauSoDiDong.Length; i++)
            {                
                pattern += string.Format(@"{0}\d", dauSoDiDong[i]);
                pattern += "{7}";
                if (i != dauSoDiDong.Length - 1)
                {
                    pattern += "|";
                }

            }

            var regex = new Regex(pattern);
            return regex.IsMatch(s);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


    }
}