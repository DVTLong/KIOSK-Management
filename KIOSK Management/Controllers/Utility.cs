using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}