using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akgul_Yemek.Models
{
    public class SmsModel
    {
        // just for redirections
        public int org_info_id { get; set; }



        public string phoneNumber { get; set; }
        public string content { get; set; }

        public bool isValid()
        {
            if (GetPhoneNumber().Length == 10)
                return true;
            else
                return false;
        }
        public string GetPhoneNumber()
        {
            if (phoneNumber.StartsWith("0"))
                return phoneNumber.Substring(1, phoneNumber.Length - 1);
            else
                return phoneNumber;
        }
        public string GetContent()
        {
            content = content.Replace("Ğ", "G");
            content = content.Replace("ğ", "g");
            content = content.Replace("Ü", "U");
            content = content.Replace("ü", "u");
            content = content.Replace("Ş", "S");
            content = content.Replace("ş", "s");
            content = content.Replace("İ", "I");
            content = content.Replace("ı", "i");
            content = content.Replace("Ö", "O");
            content = content.Replace("ö", "o");
            content = content.Replace("Ç", "C");
            content = content.Replace("ç", "c");

            content = content.Replace("\r", "");

            return content;
        }
    }
}