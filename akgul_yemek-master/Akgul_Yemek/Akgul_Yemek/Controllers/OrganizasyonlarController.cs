using Akgul_Yemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akgul_Yemek.Controllers
{
    public class OrganizasyonlarController : Controller
    {
        //
        // GET: /Organizasyonlar/

        public ActionResult Listele()
        {
            if (Session["type"] == null)
                Response.Redirect("/Uye/Giris");

            bool isAdmin = true;
            if (Session["type"].ToString() == "1")
                isAdmin = false;

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_information> org_infos = db.organization_information.ToList();

            List<organization_information> today_org_infos = new List<organization_information>();
            List<organization_information> future_org_infos = new List<organization_information>();

            List<organization_information> nonAccept_org_infos = new List<organization_information>();
            List<organization_information> finished_org_infos = new List<organization_information>();

            List<organization_information> completely_finished_org_infos = new List<organization_information>();

            foreach (organization_information org_inf in org_infos)
            {
                // Accepted org.s Transition_OrganizasyonDurumunuBittiOlarakDegistir
                if (org_inf.organization_status == 1)
                {
                    if (org_inf.date > DateTime.Now)
                    {
                        TimeSpan tsp = org_inf.date - DateTime.Now;
                        if (tsp.Days == 0)
                        {
                            today_org_infos.Add(org_inf);
                        }
                        if (tsp.Days > 0)
                        {
                            if (isAdmin)
                            {
                                future_org_infos.Add(org_inf);
                            }
                            else
                            {
                                // take org_inf if it is on the next week
                                if (tsp.Days < 8)
                                {
                                    future_org_infos.Add(org_inf);
                                }
                            }

                        }
                    }
                    else if (org_inf.date < DateTime.Now)
                    {
                        finished_org_infos.Add(org_inf);
                    }
                }
                else if (org_inf.organization_status == 0) // nonAccepted orgs
                {
                    nonAccept_org_infos.Add(org_inf);
                }
                else if (org_inf.organization_status == 2) // completely finished org.
                {
                    //completely_finished_org_infos.Add(org_inf);
                }
            }

            ViewData["today_org_infos"] = today_org_infos.OrderBy(w => w.date).ToList();
            ViewData["future_org_infos"] = future_org_infos.OrderBy(w => w.date).ToList();
            ViewData["finished_org_infos"] = finished_org_infos.OrderByDescending(w => w.date).ToList();
            ViewData["nonAccept_org_infos"] = nonAccept_org_infos.OrderBy(w => w.date).ToList();
            //ViewData["completely_finished_org_infos"] = completely_finished_org_infos.OrderBy(w => w.date).ToList();

            return View();
        }


        public ActionResult CiktiSayfasi(string id)
        {
            int org_info_id = Convert.ToInt32(id);

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
            organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            List<organization_service> services = db.organization_service.ToList();
            ViewData["services"] = services;

            List<organization_and_food> org_and_foods = db.organization_and_food.Where(w => w.organization_information_id == org_info_id).ToList();
            ViewData["org_and_foods"] = org_and_foods;

            List<organization_and_service> org_and_service = db.organization_and_service.Where(w => w.organization_information_id == org_info_id).ToList();
            ViewData["org_and_service"] = org_and_service;

            int first_remenu_id = db.ready_menu.ToList().FirstOrDefault().id;
            ViewData["first_remenu_id"] = first_remenu_id;

            return View();
        }

        public ActionResult YetkiliCiktiSayfasi(string id)
        {
            int org_info_id = Convert.ToInt32(id);

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
            organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            List<organization_service> services = db.organization_service.ToList();
            ViewData["services"] = services;

            List<organization_and_food> org_and_foods = db.organization_and_food.Where(w => w.organization_information_id == org_info_id).ToList();
            ViewData["org_and_foods"] = org_and_foods;

            List<organization_and_service> org_and_service = db.organization_and_service.Where(w => w.organization_information_id == org_info_id).ToList();
            ViewData["org_and_service"] = org_and_service;

            int first_remenu_id = db.ready_menu.ToList().FirstOrDefault().id;
            ViewData["first_remenu_id"] = first_remenu_id;

            return View();
        }

    }
}
