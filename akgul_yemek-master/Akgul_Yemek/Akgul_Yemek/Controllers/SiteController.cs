using Akgul_Yemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akgul_Yemek.Code;

namespace Akgul_Yemek.Controllers
{
    public class SiteController : Controller
    {
        //
        // GET: /Site/

        public ActionResult Index()
        {
            return Redirect("/Uye/Giris");

            //return View();

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            ViewData["sliders"] = db.slider.ToList();


            ViewData["menus"] = db.site_menu.ToList();

            ViewData["gallery_categories"] = db.gallery_categories.ToList();

            Iletisim_Model iletisim_model = new Iletisim_Model()
            {
                address = db.details.Where(w => w.key_ == "address").FirstOrDefault().value,
                email = db.details.Where(w => w.key_ == "email").FirstOrDefault().value,
                tel1 = db.details.Where(w => w.key_ == "tel1").FirstOrDefault().value,
                tel2 = db.details.Where(w => w.key_ == "tel2").FirstOrDefault().value
            };

            IcerikModel icerikModel = new IcerikModel()
            {
                hakkimizda = db.details.Where(w => w.key_ == "hakkimizda").FirstOrDefault().value,
                sertifikalarimiz = db.details.Where(w => w.key_ == "sertifikalarimiz").FirstOrDefault().value,
                hizmetlerimiz = db.details.Where(w => w.key_ == "hizmetlerimiz").FirstOrDefault().value,
                insankaynaklari = db.details.Where(w => w.key_ == "insankaynaklari").FirstOrDefault().value
            };

            ViewData["icerikModel"] = icerikModel;
            ViewData["iletisim_model"] = iletisim_model;


            return View();
        }

        public ActionResult Transition_IletisimFormu(FormCollection formCollection)
        {
            try
            {
                // getting website's mail address from db
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                string web_site_email = db.details.Where(w => w.key_ == "email").FirstOrDefault().value;

                // fortest
                //web_site_email = "eyildizpro@gmail.com";

                // Creating a mail schema
                IletisimForm_Model iletisimForm_model = (IletisimForm_Model)MTranslation.BuildObject(formCollection, "IletisimForm_Model");

                string name = iletisimForm_model.name;
                string email = iletisimForm_model.email;
                string tel = iletisimForm_model.tel;
                string subject = iletisimForm_model.subject;
                string message = iletisimForm_model.message;

                string konu = "Akggül Yemek Web Sitesi İletisim Formu Gönderisi";

                string mesaj = "İsim : " + name + " --- " + Environment.NewLine +
                                "Tel  : " + tel + " --- " + Environment.NewLine +
                                "Email: " + email + " --- " + Environment.NewLine + Environment.NewLine +
                                "Konu : " + subject + Environment.NewLine + Environment.NewLine +
                                "Mesaj: " + message;


                // sending email:
                bool sonuc = MEmail.MailGonder(web_site_email, konu, mesaj);

                if (sonuc)
                {
                    TempData["message_model"] = new MessageModel("Sonuç", "Mesajınınız Başarıyla Sisteme İletildi", Message_Type.Success);
                }
                else
                {
                    MLog.Error("Site İletişim Formu: Sisteme mesaj iletilemedi.", "Mesajınınız Sisteme İletilirken Bir Hata Oluştu");
                    TempData["message_model"] = new MessageModel("Sonuç", "Mesajınınız Sisteme İletilirken Bir Hata Oluştu", Message_Type.Error);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Site İletişim Formu: Sisteme mesaj iletilemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                TempData["message_model"] = new MessageModel("Sonuç", "Bir Hata Oluştu", Message_Type.Error);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}
