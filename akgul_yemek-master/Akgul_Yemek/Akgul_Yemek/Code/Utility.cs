using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Akgul_Yemek.Models;
using System.Net;
using System.IO;

namespace Akgul_Yemek
{
    public class Utility
    {
        public static string SayiOlustur(int karakter_uzunlugu)
        {
            string _return = "";

            for (int i = 0; i < karakter_uzunlugu; i++)
            {
                _return += "" + new Random().Next(0, 9);
            }

            return _return;
        }

        public static string FixDate(string str_date)
        {
            string _return = "";

            string[] pieces = str_date.Split('.');
            for (int i = 0; i < pieces.Length; i++)
            {
                string piece = pieces.ElementAt(i);

                if (i != 0)
                    _return += ".";

                if (piece.Length < 2)
                {
                    _return += "0" + piece;
                }
                else
                {
                    _return += piece;
                }
            }

            return _return;
        }
    }
}