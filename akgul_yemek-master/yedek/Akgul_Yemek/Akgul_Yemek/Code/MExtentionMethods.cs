using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Akgul_Yemek
{

    public static class MExtentionMethods
    {
        public static string CropString(this string value, int bound)
        {
            if (value.Length >= bound)
                return value.Substring(0, bound);
            else
                return value;
        }
        public static string RemoveHtml(this string value)
        {
            return Regex.Replace(value, @"<(.|\n)*?>", string.Empty);
        }

        public static DateTime ToDateTime(this string value)
        {
            string format = "";
            if (value.Trim().Length > 11)
                format = "dd.MM.yyyy hh:mm:ss";
            else
                format = "dd.MM.yyyy";

            value = Utility.FixDate(value);

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            return DateTime.ParseExact(value, format, cultureInfo, DateTimeStyles.None);
        }

        public static string ToDatetimeString(this DateTime datetime)
        {
            return datetime.ToString("dd/MM/yyyy hh:mm");
        }

        public static string ToDateString(this DateTime datetime)
        {
            return datetime.ToString("dd/MM/yyyy");
        }


        
    }
}