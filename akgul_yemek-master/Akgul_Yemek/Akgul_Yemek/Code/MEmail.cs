using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Akgul_Yemek.Code
{
    public class MEmail
    {
        public static bool MailGonder(string kime, string konu, string icerik, bool isBodyHtml = true)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress(Important_Things.mail_adresi);
            //
            ePosta.To.Add(kime);
            //
            //ePosta.Attachments.Add(new Attachment(@"C:\deneme.txt"));
            //
            ePosta.Subject = konu;
            //
            ePosta.Body = icerik;
            //
            ePosta.IsBodyHtml = isBodyHtml;
            //
            SmtpClient smtp = new SmtpClient();
            //
            smtp.Credentials = new System.Net.NetworkCredential(Important_Things.mail_adresi, Important_Things.mail_sifresi);
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            bool kontrol = true;

            try
            {
                smtp.SendAsync(ePosta, (object)ePosta);
            }
            catch (SmtpException ex)
            {
                kontrol = false;
                string error_message = ex.Message;
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Mail Gönderme Hatasi");
            }

            return kontrol;
        }

    }
}