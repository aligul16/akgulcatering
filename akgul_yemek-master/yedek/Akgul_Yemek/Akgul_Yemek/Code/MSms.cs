using Akgul_Yemek.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Akgul_Yemek.Code
{
    public class MSms
    {
        // It works just with NETGSM Operator
        public static bool SmsGonder(SmsModel smsModel)
        {
            bool kontrol = false;

            try
            {
                string username = "5071145040";
                string password = "5071145040";
                string baslik = "AKGUL YEMEK";
                string gsmNo = smsModel.GetPhoneNumber();
                string content = smsModel.GetContent();

                baslik = baslik.Replace(" ", "%20");
                content = content.Replace(" ", "%20");
                string datetime = DateTime.Now.ToString().Replace(":", "").Replace(".", "").Replace(" ", "");
                datetime = datetime.Substring(0, datetime.Length - 2);

                string html = string.Empty;
                string url = @"https://api.netgsm.com.tr/sms/send/get/?usercode=" + username + "&password=" + password + "&gsmno=" + gsmNo + "&message=" + content + "&msgheader=" + baslik + "&startdate=" + datetime + "&stopdate=" + datetime + "&dil=TR";
                Console.WriteLine(url);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                if (html != "20" && html != "30" && html != "40" && html != "70")
                {
                    // success
                    // Console.WriteLine("SUCCEDDED:" + html);
                    kontrol = true;
                }
                else
                {
                    // failed
                    // Console.WriteLine("FAILED:" + html);
                    kontrol = false;
                }
            }
            catch// (Exception exception)
            {
                //Console.WriteLine("HATA:::: " + exception.Message);
                kontrol = false;
            }

            return kontrol;
        }

    }
}