using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akgul_Yemek.Models;
using Akgul_Yemek.Code;
using Newtonsoft.Json;

namespace Akgul_Yemek.Controllers
{
    public class UyeController : Controller
    {
        //
        // GET: /Uye/

        public ActionResult Giris(string id)
        {
            if (id== "fromLogout")
            {
                // this page called from Logout Page
            }
            else
            {
                // for showing message | invalid, error
                ViewData["id"] = id;

                if (Request.Cookies["user"] != null)
                {
                    string strData = Request.Cookies["user"].Value;
                    user _user = (user)JsonConvert.DeserializeObject<user>(strData);

                    TempData[_user.telephone] = JsonConvert.SerializeObject(_user);
                    TempData["isRememberMe"] = "off";

                    return RedirectToAction("Login", new { id = _user.telephone });
                }
            }

            return View();
        }



        [HttpPost]
        public ActionResult Transition_Giris(FormCollection formCollection)
        {
            try
            {
                user _user = (user)MTranslation.BuildObject(formCollection, "user");

                // adding zero(0) to heading of telephone number if user not entered number starting with zero
                _user.telephone = _user.telephone.StartsWith("0") ? _user.telephone : "0" + _user.telephone;

                TempData[_user.telephone] = JsonConvert.SerializeObject(_user);
                string data = formCollection["isRememberMe"].ToString();
                TempData["isRememberMe"] = data;

                return RedirectToAction("Login", new { id = _user.telephone });
            }
            catch (Exception exception)
            {
                MLog.Error("Giriş Başarısız Oldu", exception.Message + Environment.NewLine + exception.StackTrace);
                TempData["login_message"] = "Bir hata oluştu.";

                return RedirectToAction("Giris");
            }
        }

        // this will call from User login page and AutoLogin by cookie
        public ActionResult Login(string id)
        {
            user _user = (user)JsonConvert.DeserializeObject<user>(TempData[id].ToString());
            TempData[id] = null;

            bool isRememberMe = (TempData["isRememberMe"].ToString() == "on") ? true : false;
            TempData["isRememberMe"] = null;

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            user mapped_user = db.user.Where(w => w.telephone == _user.telephone && w.password == _user.password).FirstOrDefault();

            if (mapped_user != null)
            {
                Session.Add("telephone", mapped_user.telephone);
                Session.Add("name", mapped_user.name);
                Session.Add("job", mapped_user.job);
                Session.Add("type", "" + mapped_user.type); // 0 admin, 1 normal

                string isAdmin = (mapped_user.type == 0) ? "true" : "false";
                Session.Add("isAdmin", isAdmin);

                // if user selected Remember Me checkbox
                if (isRememberMe)
                {
                    var json = JsonConvert.SerializeObject(_user);

                    var userCookie = new HttpCookie("user", json);
                    userCookie.Expires.AddDays(365);
                    HttpContext.Response.SetCookie(userCookie);
                }

                return Redirect("/Organizasyonlar/Listele");
            }
            else
            {
                MLog.Warning("Giriş Başarısız Oldu", "Telefon numarası veya şifre yanlış." + _user.telephone + " " + _user.password);
                TempData["login_message"] = "Telefon numarası veya şifre yanlış.";

                return RedirectToAction("Giris");
            }
        }



        public ActionResult Cikis()
        {
            Session.RemoveAll();

            return RedirectToAction("Giris", new { id = "fromLogout" });
        }


    }
}
