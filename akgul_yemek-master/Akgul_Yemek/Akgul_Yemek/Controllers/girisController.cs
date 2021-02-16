using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akgul_Yemek.Controllers
{
    public class girisController : Controller
    {
        //
        // GET: /giris/

        public ActionResult Index()
        {
            return Redirect("/Uye/Giris");
        }

    }
}
