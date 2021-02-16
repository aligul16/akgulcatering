using Akgul_Yemek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Akgul_Yemek.Code;
using System.IO;

namespace Akgul_Yemek.Controllers
{
    public class Admin_SistemController : Controller
    {

        #region Statistics

        public ActionResult Index()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_information> org_infos = db.organization_information.ToList();

            List<organization_information> today_org_infos = new List<organization_information>();
            List<organization_information> future_org_infos = new List<organization_information>();

            List<organization_information> nonAccept_org_infos = new List<organization_information>();
            List<organization_information> finished_org_infos = new List<organization_information>();

            List<organization_information> completely_finished_org_infos = new List<organization_information>();

            foreach (organization_information org_inf in org_infos)
            {
                // Accepted org.s
                if (org_inf.organization_status == 1)
                {
                    if (org_inf.date > DateTime.Now)
                    {
                        TimeSpan tsp = org_inf.date - DateTime.Now;
                        if (tsp.Days == 0)
                            today_org_infos.Add(org_inf);
                        if (tsp.Days > 0)
                            future_org_infos.Add(org_inf);
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
                    completely_finished_org_infos.Add(org_inf);
                }
            }


            // assing statistics
            Admin_Istatistik_Model admin_istatistik = new Admin_Istatistik_Model()
            {
                finished_org_count = finished_org_infos.Count,
                future_org_count = future_org_infos.Count,
                notApplied_org_count = nonAccept_org_infos.Count,
                completely_finished_org_count = completely_finished_org_infos.Count,
                food_count = db.food.Count(),
                ready_menu_count = db.ready_menu.Count(),
                username = Session["name"].ToString()
            };

            ViewData["statistics"] = admin_istatistik;



            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________




            return View();
        }

        #endregion


        #region Yemek: Kategori/Yemek

        public ActionResult Yemek_Kategorisi_Ekle()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<food_category> food_categories = db.food_category.ToList();
            ViewData["food_categories"] = food_categories;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="YEMEK KATEGORİSİ EKLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        [HttpPost]
        public ActionResult Transition_YemekKategoriEkle(FormCollection formCollection)
        {
            try
            {
                food_category f_category = (food_category)MTranslation.BuildObject(formCollection, "food_category");
                if (f_category.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.food_category.Add(f_category);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK KATEGORİSİ EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("Yemek_Kategorisi_Ekle");
                }
                else
                {
                    MLog.Error("Yemek Kategorisi Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Yemek_Kategorisi_Ekle");
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Yemek Kategorisi Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Yemek_Kategorisi_Ekle");
            }
        }

        [HttpPost]
        public ActionResult Transition_YemekKategoriDuzenle(FormCollection formCollection)
        {

            int category_id = Convert.ToInt32(formCollection["id"]);
            try
            {
                food_category f_category = (food_category)MTranslation.BuildObject(formCollection, "food_category");
                if (f_category.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    food_category mapped_f_category = db.food_category.Where(w => w.id == f_category.id).FirstOrDefault();

                    // updating
                    mapped_f_category.name = f_category.name;

                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK KATEGORİSİ GÜNCELLENDİ.", Message_Type.Success);
                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = mapped_f_category.id });
                }
                else
                {
                    MLog.Error("Kategoriye Yemek Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = f_category.id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Kategoriye Yemek Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
            }
        }

        public ActionResult Transition_YemekKategoriSil(string id)
        {
            try
            {
                int f_category_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                food_category f_category = db.food_category.Where(w => w.id == f_category_id).FirstOrDefault();
                if (f_category != null)
                {
                    db.food_category.Remove(f_category);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK KATEGORİSİ SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("Yemek_Kategorisi_Ekle");
                }
                else
                {
                    MLog.Error("Yemek Kategori Silinemedi", "Kategori Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "KATEGORİ BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("Yemek_Kategorisi_Ekle");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Yemek Kategori Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Yemek_Kategorisi_Ekle");
            }
        }

        //____________________________________________________________________________________________

        public ActionResult Kategoriye_Yemek_Ekle(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            int f_category_id = Convert.ToInt32(id);
            food_category f_category = db.food_category.Where(w => w.id == f_category_id).FirstOrDefault();
            ViewData["f_category"] = f_category;

            List<food> foods = db.food.Where(w => w.category_id == f_category_id).ToList();
            ViewData["foods"] = foods;

            List<food_units> _units = db.food_units.ToList();
            ViewData["_units"] = _units;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="YEMEK KATEGORİSİ EKLE",url="/Admin_Sistem/Yemek_Kategorisi_Ekle"},
                new NavigationModel(){name="KATEGORİYE YEMEK EKLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        [HttpPost]
        public ActionResult Transition_KategoriyeYemekEkle(FormCollection formCollection)
        {
            int category_id = Convert.ToInt32(formCollection["category_id"].ToString());

            try
            {
                food _food = (food)MTranslation.BuildObject(formCollection, "food");
                if (_food.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.food.Add(_food);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
                }
                else
                {
                    MLog.Error("Kategoriye Yemek Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Kategoriye Yemek Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
            }
        }

        public ActionResult Transition_KategoriYemekSil(string id)
        {
            int food_id = Convert.ToInt32(id);
            int category_id = 0;
            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                food _food = db.food.Where(w => w.id == food_id).FirstOrDefault();
                category_id = _food.category_id;
                if (_food != null)
                {
                    db.food.Remove(_food);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
                }
                else
                {
                    MLog.Error("Kategoriden Yemek Silinemedi", "Yemek Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "YEMEK BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Kategoriden Yemek Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU. Hazır Menu bu yemeği içeriyor olabilir, önce hazır menüden bu yemeği silin.", Message_Type.Error);

                return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = category_id });
            }
        }

        [HttpPost]
        public ActionResult Transition_KategoriYemekDuzenle(FormCollection formCollection)
        {
            food _food = null;

            try
            {
                _food = (food)MTranslation.BuildObject(formCollection, "food");
                if (_food.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    food mapped_food = db.food.Where(w => w.id == _food.id).FirstOrDefault();

                    // updating
                    mapped_food.name = _food.name;
                    mapped_food.price = _food.price;
                    mapped_food.quantity = _food.quantity;
                    mapped_food.unit_id = _food.unit_id;

                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK KATEGORİSİ GÜNCELLENDİ.", Message_Type.Success);
                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = mapped_food.id });
                }
                else
                {
                    MLog.Error("Kategorideki Yemek Düzenlenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = _food.id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Kategorideki Yemek Düzenlenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Kategoriye_Yemek_Ekle", new { id = _food.id });
            }
        }


        #endregion


        #region HazırMenu

        public ActionResult Hazir_Menu_Ekle()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<ready_menu> ready_menus = db.ready_menu.ToList();
            ViewData["ready_menus"] = ready_menus;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="HAZIR MENÜ EKLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        public ActionResult Transition_HazirMenuSil(string id)
        {
            try
            {
                int menu_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                ready_menu remenu = db.ready_menu.Where(w => w.id == menu_id).FirstOrDefault();
                if (remenu != null)
                {
                    db.ready_menu.Remove(remenu);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "HAZIR MENU SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("Hazir_Menu_Ekle");
                }
                else
                {
                    MLog.Error("Hazır Menü Silinemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "MENU BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("Hazir_Menu_Ekle");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Hazır Menü Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Hazir_Menu_Ekle");
            }
        }

        [HttpPost]
        public ActionResult Transition_HazirMenuDuzenle(FormCollection formCollection)
        {
            ready_menu remenu = null;

            try
            {
                remenu = (ready_menu)MTranslation.BuildObject(formCollection, "ready_menu");
                if (remenu.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    ready_menu mapped_remenu = db.ready_menu.Where(w => w.id == remenu.id).FirstOrDefault();

                    // updating
                    mapped_remenu.name = remenu.name;

                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK KATEGORİSİ GÜNCELLENDİ.", Message_Type.Success);
                    return RedirectToAction("Hazir_Menu_Duzenle", new { id = mapped_remenu.id });
                }
                else
                {
                    MLog.Error("Hazır Menü Düzenlenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu.id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Hazır Menü Düzenlenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu.id });
            }
        }

        [HttpPost]
        public ActionResult Transition_HazirMenuEkle(FormCollection formCollection)
        {
            try
            {
                ready_menu remenu = (ready_menu)MTranslation.BuildObject(formCollection, "ready_menu");
                if (remenu.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.ready_menu.Add(remenu);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "HAZIR MENU EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("Hazir_Menu_Ekle");
                }
                else
                {
                    MLog.Error("Hazır Menü Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Hazir_Menu_Ekle");
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Hazır Menü Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Hazir_Menu_Ekle");
            }
        }

        //______________________________________________________________________________________________



        public ActionResult Hazir_Menu_Duzenle(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            int menu_id = Convert.ToInt32(id);
            ready_menu remenu = db.ready_menu.Where(w => w.id == menu_id).FirstOrDefault();
            ViewData["remenu"] = remenu;

            List<ready_menu_and_food> ready_menu_and_foods = db.ready_menu_and_food.Where(w => w.ready_menu_id == menu_id).ToList();
            ViewData["ready_menu_and_foods"] = ready_menu_and_foods;

            List<ext_CategoryAndFood> categories_and_foods = new List<ext_CategoryAndFood>();

            // for each food_category find it's foods
            List<food_category> food_categories = db.food_category.ToList();
            ViewData["f_categories"] = food_categories;
            foreach (food_category f_category in food_categories)
            {
                List<food> _foods = db.food.Where(w => w.category_id == f_category.id).ToList();

                ext_CategoryAndFood category_and_food = new ext_CategoryAndFood()
                {
                    foods = _foods,
                    f_category = f_category
                };

                categories_and_foods.Add(category_and_food);
            }

            ViewData["categories_and_foods"] = categories_and_foods;


            List<food_units> _units = db.food_units.ToList();
            ViewData["units"] = _units;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="HAZIR MENÜ EKLE",url="/Admin_Sistem/Hazir_Menu_Ekle"},
                new NavigationModel(){name="HAZIR MENÜ DÜZENLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        [HttpPost]
        public ActionResult Transition_HazirMenuyeYemekEkle(FormCollection formCollection)
        {
            int remenu_id = Convert.ToInt32(formCollection["ready_menu_id"].ToString());

            try
            {
                ready_menu_and_food menu_and_food = (ready_menu_and_food)MTranslation.BuildObject(formCollection, "ready_menu_and_food");

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();


                // deleting old foods whichs are have the same category in the same menu
                //int food_category_id = db.food.Where(w => w.id == menu_and_food.food_id).FirstOrDefault().category_id;
                //List<ready_menu_and_food> old_foods = db.ready_menu_and_food.Where(w => w.ready_menu_id == remenu_id && w.food.category_id == food_category_id).ToList();
                //foreach (ready_menu_and_food _food in old_foods)
                //{
                //    db.ready_menu_and_food.Remove(_food);
                //}
                //db.SaveChanges();


                // adding food to menu
                db.ready_menu_and_food.Add(menu_and_food);
                db.SaveChanges();

                Session["message"] = new MessageModel("Bilgi", "YEMEK MENUYE EKLENDİ.", Message_Type.Success);
                return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu_id });


            }
            catch (Exception exception)
            {
                MLog.Error("Menüye Yemek Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu_id });
            }
        }

        [HttpPost]
        public ActionResult Transition_HazirMenuYemekDuzenle(FormCollection formCollection)
        {
            ready_menu_and_food remenuFood = null;

            try
            {
                remenuFood = (ready_menu_and_food)MTranslation.BuildObject(formCollection, "ready_menu_and_food");

                if (remenuFood != null)
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    ready_menu_and_food mapped_remenuAndFood = db.ready_menu_and_food.FirstOrDefault(w => w.id == remenuFood.id);

                    // updating
                    mapped_remenuAndFood.price = remenuFood.price;
                    mapped_remenuAndFood.quantity = remenuFood.quantity;

                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "MENÜ YEMEĞİ GÜNCELLENDİ.", Message_Type.Success);
                    return RedirectToAction("Hazir_Menu_Duzenle", new { id = mapped_remenuAndFood.ready_menu_id });
                }
                else
                {
                    MLog.Error("Hazır Menü Yemeği Düzenlenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenuFood.ready_menu_id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Hazır Menü Yemeği Düzenlenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenuFood.ready_menu_id });
            }
        }


        public ActionResult Transition_HazirMenuYemekSil(string id)
        {

            string[] ids = id.Split('-');

            int remenu_id = Convert.ToInt32(ids[0]);
            int remenu_food_id = Convert.ToInt32(ids[1]);

            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                ready_menu_and_food remenu_food = db.ready_menu_and_food.Where(w => w.id == remenu_food_id).FirstOrDefault();
                if (remenu_food != null)
                {
                    db.ready_menu_and_food.Remove(remenu_food);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "HAZIR MENU YEMEK SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu_id });
                }
                else
                {
                    MLog.Error("Hazır Menüden Yemek Silinemedi", "Menü Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "MENU BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Hazır Menüden Yemek Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Hazir_Menu_Duzenle", new { id = remenu_id });
            }
        }

        #endregion


        #region OrganizasyonTipi

        public ActionResult Organizasyon_Tipi_Ekle_Sil()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_type> types = db.organization_type.ToList();
            ViewData["types"] = types;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORGANİZASYON TİPİ EKLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        [HttpPost]
        public ActionResult Transition_OrganizasyonTipiEkle(FormCollection formCollection)
        {
            try
            {
                organization_type org_type = (organization_type)MTranslation.BuildObject(formCollection, "organization_type");
                if (org_type.name != "")
                {
                    org_type.color = "RED";

                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.organization_type.Add(org_type);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON TİPİ EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("Organizasyon_Tipi_Ekle_Sil");
                }
                else
                {
                    MLog.Error("Organizasyon Tipi Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Organizasyon_Tipi_Ekle_Sil");
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Tipi Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Organizasyon_Tipi_Ekle_Sil");
            }
        }


        public ActionResult Transition_OrganizasyonTipiSil(string id)
        {
            try
            {
                int org_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                organization_type org_type = db.organization_type.Where(w => w.id == org_id).FirstOrDefault();
                if (org_type != null)
                {
                    db.organization_type.Remove(org_type);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON TİPİ SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("Organizasyon_Tipi_Ekle_Sil");
                }
                else
                {
                    MLog.Error("Organizasyon Tipi Silinemedi", "Kategori Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "KATEGORİ BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("Organizasyon_Tipi_Ekle_Sil");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Tipi Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Organizasyon_Tipi_Ekle_Sil");
            }
        }

        #endregion


        #region Organizasyon

        public ActionResult SiparisAl_Yemek(string id)
        {
            int org_info_id;
            int ready_menu_id = -1;

            if (id.IndexOf('_') != -1)
            {
                org_info_id = Convert.ToInt32(id.Split('_')[0]);

                ready_menu_id = Convert.ToInt32(id.Split('_')[1]);
            }
            else
            {
                org_info_id = Convert.ToInt32(id.Split('_')[0]);
            }

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
            organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            List<ready_menu> remenus = db.ready_menu.ToList();
            ViewData["remenus"] = remenus;

            List<food_category> f_categories = db.food_category.ToList();
            ViewData["f_categories"] = f_categories;

            List<food> foods = db.food.ToList();
            ViewData["foods"] = foods;

            List<ready_menu_and_food> remenu_and_foods = null;
            ready_menu remenu = null;
            if (ready_menu_id != -1)
            {
                remenu = db.ready_menu.Where(w => w.id == ready_menu_id).FirstOrDefault();
                remenu_and_foods = db.ready_menu_and_food.Where(w => w.ready_menu_id == ready_menu_id).ToList();
            }
            ViewData["remenu"] = remenu;
            ViewData["remenu_and_foods"] = remenu_and_foods;

            List<organization_and_food> org_and_foods = db.organization_and_food.Where(w => w.organization_information_id == org_info_id).ToList();
            ViewData["org_and_foods"] = org_and_foods;

            List<ext_CategoryAndFood> categories_and_foods = new List<ext_CategoryAndFood>();
            // for each food_category find it's foods
            List<food_category> food_categories = db.food_category.ToList();
            foreach (food_category f_category in food_categories)
            {
                List<food> _foods = db.food.Where(w => w.category_id == f_category.id).ToList();

                ext_CategoryAndFood category_and_food = new ext_CategoryAndFood()
                {
                    foods = _foods,
                    f_category = f_category
                };

                categories_and_foods.Add(category_and_food);
            }

            ViewData["categories_and_foods"] = categories_and_foods;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. EKLE - DÜZENLE",url="/Admin_Sistem/OnKayit_EkleDuzenle/"+org_info_id},
                new NavigationModel(){name="YEMEK",url=""},
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        public ActionResult Transition_SiparisTumMenudekiYemekleriEkle(string id)
        {
            try
            {
                int org_info_id;
                int ready_menu_id = -1;

                if (id.IndexOf('_') == -1)
                {
                    MLog.Error("Organizasyona Tüm Menüdeki Yemekler Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("SiparisAl_Yemek", new { id = id });
                }
                else
                {
                    org_info_id = Convert.ToInt32(id.Split('_')[0]);

                    ready_menu_id = Convert.ToInt32(id.Split('_')[1]);

                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    organization_information org_info = db.organization_information.FirstOrDefault(w => w.id == org_info_id);
                    List<ready_menu_and_food> remenu_and_foods = db.ready_menu_and_food.Where(w => w.ready_menu_id == ready_menu_id).ToList();

                    foreach (ready_menu_and_food remenu_and_food in remenu_and_foods)
                    {
                        organization_and_food org_and_food = new organization_and_food()
                        {
                            food_id = remenu_and_food.food_id,
                            organization_information_id = org_info.id,
                            people_count = org_info.people_count,
                            price = remenu_and_food.price,
                            quantity = remenu_and_food.quantity,
                            total_price = remenu_and_food.price * org_info.people_count
                        };
                        db.organization_and_food.Add(org_and_food);
                    }
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Tüm menüdeki yemekler eklendi.", Message_Type.Success);
                    return RedirectToAction("SiparisAl_Yemek", new { id = org_info_id + "_" + ready_menu_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyona Tüm Menüdeki Yemekler Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("SiparisAl_Yemek");
            }
        }

        [HttpPost]
        public ActionResult Transition_SiparisYemekEkle(FormCollection formCollection)
        {
            string remenu_id = formCollection["remenu_id"].ToString();
            string org_info_id = formCollection["organization_information_id"].ToString();
            string route_parameters = org_info_id + "_" + remenu_id;

            try
            {
                organization_and_food org_and_food = (organization_and_food)MTranslation.BuildObject(formCollection, "organization_and_food");
                if (org_and_food.organization_information_id > 0)
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    // calculating...
                    org_and_food.total_price = org_and_food.price * org_and_food.people_count;

                    db.organization_and_food.Add(org_and_food);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("SiparisAl_Yemek", new { id = route_parameters });
                }
                else
                {
                    MLog.Error("Organizasyona Yemek Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("SiparisAl_Yemek", new { id = route_parameters });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyona Yemek Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("SiparisAl_Yemek", new { id = route_parameters });
            }
        }

        [HttpPost]
        public ActionResult Transition_SiparisYemekDuzenle(FormCollection formCollection)
        {
            string remenu_id = formCollection["remenu_id"].ToString();
            string org_info_id = formCollection["organization_information_id"].ToString();
            string route_parameters = org_info_id + "_" + remenu_id;

            try
            {
                organization_and_food org_and_food = (organization_and_food)MTranslation.BuildObject(formCollection, "organization_and_food");
                if (org_and_food.organization_information_id > 0)
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    organization_and_food mappedOrgAndFood = db.organization_and_food.FirstOrDefault(w => w.id == org_and_food.id);

                    mappedOrgAndFood.people_count = org_and_food.people_count;
                    mappedOrgAndFood.price = org_and_food.price;
                    mappedOrgAndFood.quantity = org_and_food.quantity;
                    // calculating...
                    mappedOrgAndFood.total_price = org_and_food.price * org_and_food.people_count;

                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK DÜZENLENDİ.", Message_Type.Success);
                    return RedirectToAction("SiparisAl_Yemek", new { id = route_parameters });
                }
                else
                {
                    MLog.Error("Organizasyon Yemeği Düzenlenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("SiparisAl_Yemek", new { id = route_parameters });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyona Yemek Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("SiparisAl_Yemek", new { id = route_parameters });
            }
        }

        public ActionResult Transition_SiparisYemekSil(string id)
        {
            int org_and_food_id = Convert.ToInt32(id.Split('_')[0]);
            int remenu_id = Convert.ToInt32(id.Split('_')[1]);

            int org_info_id = 0;

            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                organization_and_food org_and_food = db.organization_and_food.Where(w => w.id == org_and_food_id).FirstOrDefault();
                if (org_and_food != null)
                {
                    org_info_id = org_and_food.organization_information_id;

                    db.organization_and_food.Remove(org_and_food);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "YEMEK SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("SiparisAl_Yemek", new { id = org_info_id + "_" + remenu_id });
                }
                else
                {
                    MLog.Error("Organizasyondan Yemek Silinemedi", "Yemek Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "YEMEK BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("SiparisAl_Yemek", new { id = org_info_id + "_" + remenu_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyondan Yemek Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("SiparisAl_Yemek", new { id = org_info_id + "_" + remenu_id });
            }
        }

        [HttpPost]
        public ActionResult Transition_SiparisPersonelGuncelle(FormCollection formCollection)
        {
            organization_information newOrgInfo = null;

            try
            {
                newOrgInfo = (organization_information)MTranslation.BuildObject(formCollection, "organization_information");
                if (newOrgInfo.discount >= 0 && newOrgInfo.count_cook >= 0 && newOrgInfo.count_chef >= 0 && newOrgInfo.count_waiter >= 0 && newOrgInfo.count_other >= 0)
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    organization_information oldOrgInfo = db.organization_information.Where(w => w.id == newOrgInfo.id).FirstOrDefault();
                    if (oldOrgInfo != null)
                    {
                        oldOrgInfo.count_chef = newOrgInfo.count_chef;
                        oldOrgInfo.count_cook = newOrgInfo.count_cook;
                        oldOrgInfo.count_other = newOrgInfo.count_other;
                        oldOrgInfo.count_waiter = newOrgInfo.count_waiter;

                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", "Personel Sayısı Güncellendi.", Message_Type.Success);
                        return RedirectToAction("SiparisAl_Personel", new { id = newOrgInfo.id });
                    }
                    else
                    {
                        MLog.Error("Personel Sayısı Güncellenemedi", "Organizasyon Bulunamadı.");
                        Session["message"] = new MessageModel("HATA", "Organizasyon bulunamadı.", Message_Type.Error);

                        return RedirectToAction("SiparisAl_Personel", new { id = newOrgInfo.id });
                    }
                }
                else
                {
                    MLog.Error("Personel Sayısı Güncellenemedi", "Girilen Değerlerde Hatalar Var.");
                    Session["message"] = new MessageModel("HATA", "Girilen Değerlerde Hatalar Var..", Message_Type.Error);

                    return RedirectToAction("SiparisAl_Personel", new { id = newOrgInfo.id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Personel Sayısı Güncellenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("SiparisAl_Personel", new { id = newOrgInfo.id });
            }
        }

        [HttpPost]
        public ActionResult Transition_SiparisIndirimGuncelle(FormCollection formCollection)
        {
            organization_information newOrgInfo = null;
            int remenuId = 2;
            string targetAction = "";

            try
            {
                remenuId = Convert.ToInt32(formCollection["remenu_id"]);
                targetAction = formCollection["from"];

                newOrgInfo = (organization_information)MTranslation.BuildObject(formCollection, "organization_information");
                if (newOrgInfo.discount >= 0 && newOrgInfo.count_cook >= 0 && newOrgInfo.count_chef >= 0 && newOrgInfo.count_waiter >= 0 && newOrgInfo.count_other >= 0)
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    organization_information oldOrgInfo = db.organization_information.Where(w => w.id == newOrgInfo.id).FirstOrDefault();
                    if (oldOrgInfo != null)
                    {
                        oldOrgInfo.discount = newOrgInfo.discount;

                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", "İndirim Tutarı Güncellendi.", Message_Type.Success);
                        if (targetAction == "yemek_ekle_duzenle")
                            return RedirectToAction("SiparisAl_Yemek", new { id = newOrgInfo.id + "_" + remenuId });
                        else
                            return RedirectToAction("Siparis_Detay", new { id = newOrgInfo.id });
                    }
                    else
                    {
                        MLog.Error("İndirim Tutarı Güncellenemedi", "Organizasyon Bulunamadı veya Eksik Bilgi Girildi.");
                        Session["message"] = new MessageModel("HATA", "Organizasyon Bulunamadı veya Eksik Bilgi Girildi.", Message_Type.Error);

                        if (targetAction == "yemek_ekle_duzenle")
                            return RedirectToAction("SiparisAl_Yemek", new { id = newOrgInfo.id + "_" + remenuId });
                        else
                            return RedirectToAction("Siparis_Detay", new { id = newOrgInfo.id });
                    }
                }
                else
                {
                    MLog.Error("İndirim Tutarı Güncellenemedi", "Girilen Değerlerde Hatalar Var.");
                    Session["message"] = new MessageModel("HATA", "Girilen Değerlerde Hatalar Var..", Message_Type.Error);

                    if (targetAction == "yemek_ekle_duzenle")
                        return RedirectToAction("SiparisAl_Yemek", new { id = newOrgInfo.id + "_" + remenuId });
                    else
                        return RedirectToAction("Siparis_Detay", new { id = newOrgInfo.id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("İndirim Tutarı Güncellenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                if (targetAction == "yemek_ekle_duzenle")
                    return RedirectToAction("SiparisAl_Yemek", new { id = newOrgInfo.id + "_" + remenuId });
                else
                    return RedirectToAction("Siparis_Detay", new { id = newOrgInfo.id });
            }
        }


        // masa-sandalye
        public ActionResult SiparisAl_Servis(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            int org_id = Convert.ToInt32(id);

            organization_information org_info = db.organization_information.Where(w => w.id == org_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            organization_service org_service = db.organization_service.Where(w => w.name == "Servis").FirstOrDefault();
            ViewData["org_service"] = org_service;

            organization_and_service org_and_service = db.organization_and_service.Where(w => w.organization_information_id == org_id && w.organization_service_id == org_service.id).FirstOrDefault();
            ViewData["org_and_service"] = org_and_service;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. EKLE - DÜZENLE",url="/Admin_Sistem/OnKayit_EkleDuzenle/"+org_id},
                new NavigationModel(){name="SERVİS",url=""},
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        public ActionResult SiparisAl_Emanet(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            int org_id = Convert.ToInt32(id);

            organization_information org_info = db.organization_information.Where(w => w.id == org_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            organization_service org_service = db.organization_service.Where(w => w.name == "Emanet").FirstOrDefault();
            ViewData["org_service"] = org_service;

            organization_and_service org_and_service = db.organization_and_service.Where(w => w.organization_information_id == org_id && w.organization_service_id == org_service.id).FirstOrDefault();
            ViewData["org_and_service"] = org_and_service;

            // showing emanet's content which belong a organization at the same date
            DateTime dtStart = new DateTime(org_info.date.Year, org_info.date.Month, org_info.date.Day, 0, 0, 0);
            DateTime dtStop = new DateTime(org_info.date.Year, org_info.date.Month, org_info.date.Day, 23, 59, 59);

            List<organization_information> theSameDateOrganizatations = db.organization_information.Where(w => w.date >= dtStart && w.date < dtStop && w.organization_name != org_info.organization_name && w.name_surname != org_info.name_surname).ToList();
            List<organization_and_service> theSameDateOrgServices = new List<organization_and_service>();
            foreach (organization_information orgInfo in theSameDateOrganizatations)
                theSameDateOrgServices.AddRange(orgInfo.organization_and_service.Where(w => w.organization_service.name == "Emanet"));
            ViewData["theSameDateOrgServices"] = theSameDateOrgServices;

            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. EKLE - DÜZENLE",url="/Admin_Sistem/OnKayit_EkleDuzenle/"+org_id},
                new NavigationModel(){name="EMANET",url=""},
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        public ActionResult SiparisAl_Personel(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            int org_id = Convert.ToInt32(id);

            organization_information org_info = db.organization_information.Where(w => w.id == org_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            organization_service org_service = db.organization_service.Where(w => w.name == "Personel").FirstOrDefault();
            ViewData["org_service"] = org_service;

            organization_and_service org_and_service = db.organization_and_service.Where(w => w.organization_information_id == org_id && w.organization_service_id == org_service.id).FirstOrDefault();
            ViewData["org_and_service"] = org_and_service;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. EKLE - DÜZENLE",url="/Admin_Sistem/OnKayit_EkleDuzenle/"+org_id},
                new NavigationModel(){name="PERSONEL",url=""},
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        public ActionResult SiparisAl_Aciklama(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            int org_id = Convert.ToInt32(id);

            organization_information org_info = db.organization_information.Where(w => w.id == org_id).FirstOrDefault();
            ViewData["org_info"] = org_info;

            organization_service org_service = db.organization_service.Where(w => w.name == "Aciklama").FirstOrDefault();
            ViewData["org_service"] = org_service;

            organization_and_service org_and_service = db.organization_and_service.Where(w => w.organization_information_id == org_id && w.organization_service_id == org_service.id).FirstOrDefault();
            ViewData["org_and_service"] = org_and_service;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. EKLE - DÜZENLE",url="/Admin_Sistem/OnKayit_EkleDuzenle/"+org_id},
                new NavigationModel(){name="AÇIKLAMA",url=""},
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Transition_SiparisServisEkle(FormCollection formCollection)
        {
            int org_info_id = Convert.ToInt32(formCollection["organization_information_id"].ToString());
            string current_service_name = formCollection["service_name"].ToString();
            string redirection_action = "";
            string redirection_action_error = "";

            if (current_service_name == "Servis")
            {
                redirection_action_error = "SiparisAl_Servis";
                redirection_action = "SiparisAl_Emanet";
            }
            else if (current_service_name == "Emanet")
            {
                redirection_action_error = "SiparisAl_Emanet";
                redirection_action = "SiparisAl_Personel";
            }
            else if (current_service_name == "Personel")
            {
                redirection_action_error = "SiparisAl_Personel";
                redirection_action = "SiparisAl_Aciklama";
            }
            else if (current_service_name == "Aciklama")
            {
                redirection_action_error = "SiparisAl_Aciklama";
                redirection_action = "Siparis_Detay";
            }

            try
            {
                organization_and_service org_and_service = (organization_and_service)MTranslation.BuildObject(formCollection, "organization_and_service");
                if (org_and_service.information != "")
                {
                    // ADD
                    if (org_and_service.id <= 0)
                    {
                        akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                        db.organization_and_service.Add(org_and_service);
                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", current_service_name.ToUpper() + " EKLENDİ.", Message_Type.Success);
                        return RedirectToAction(redirection_action, new { id = org_info_id });
                    }

                    // UPDATE
                    else
                    {
                        akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                        organization_and_service mapped_org_and_service = db.organization_and_service.Where(w => w.id == org_and_service.id).FirstOrDefault();

                        mapped_org_and_service.information = org_and_service.information;
                        mapped_org_and_service.price = org_and_service.price;

                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", current_service_name.ToUpper() + " GÜNCELLENDİ.", Message_Type.Success);
                        return RedirectToAction("Siparis_Detay", new { id = org_info_id });
                    }
                }
                else
                {
                    MLog.Error("Organizasyona Servis Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction(redirection_action_error, new { id = org_info_id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyona Servis Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction(redirection_action_error, new { id = org_info_id });
            }
        }



        public ActionResult Siparis_Detay(string id)
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

            // showing emanet's content which belong a organization at the same date
            DateTime dtStart = new DateTime(org_info.date.Year, org_info.date.Month, org_info.date.Day, 0, 0, 0);
            DateTime dtStop = new DateTime(org_info.date.Year, org_info.date.Month, org_info.date.Day, 23, 59, 59);
            List<organization_information> theSameDateOrganizatations = db.organization_information.Where(w => w.date >= dtStart && w.date < dtStop && w.organization_name != org_info.organization_name && w.name_surname != org_info.name_surname).ToList();
            List<organization_and_service> theSameDateOrgServices = new List<organization_and_service>();
            foreach (organization_information orgInfo in theSameDateOrganizatations)
                theSameDateOrgServices.AddRange(orgInfo.organization_and_service.Where(w => w.organization_service.name == "Emanet"));
            ViewData["theSameDateOrgServices"] = theSameDateOrgServices;

            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. DETAY",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }


        // Organizasyon Ekle Duzenle
        public ActionResult OnKayit_EkleDuzenle(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
            List<organization_type> org_types = db.organization_type.ToList();
            ViewData["org_types"] = org_types;

            if (id != null)
            {
                int org_info_id = Convert.ToInt32(id);
                organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();
                ViewData["org_info"] = org_info;
            }



            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. EKLE - DÜZENLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        [HttpPost]
        public ActionResult Transition_SiparisEkle(FormCollection formCollection)
        {
            try
            {
                organization_information org_info = (organization_information)MTranslation.BuildObject(formCollection, "organization_information");
                if (org_info.organization_name != "" && org_info.name_surname != "")
                {
                    // if user don't enter required object informations
                    org_info.email = string.IsNullOrEmpty(org_info.email) ? "belirtilmedi" : org_info.email;
                    org_info.organization_adress = string.IsNullOrEmpty(org_info.organization_adress) ? "belirtilmedi" : org_info.organization_adress;
                    org_info.organizators_adress = string.IsNullOrEmpty(org_info.organizators_adress) ? "belirtilmedi" : org_info.organizators_adress;
                    org_info.referance = string.IsNullOrEmpty(org_info.referance) ? "belirtilmedi" : org_info.referance;
                    org_info.telephone = string.IsNullOrEmpty(org_info.telephone) ? "00000000000" : org_info.telephone;
                    org_info.telephone2 = string.IsNullOrEmpty(org_info.telephone2) ? "00000000000" : org_info.telephone2;
                    org_info.time = string.IsNullOrEmpty(org_info.time) ? "belirtilmedi" : org_info.time;
                    org_info.date = (org_info.date == null) ? DateTime.Now : org_info.date;

                    // setting date
                    int hour = Convert.ToInt32(org_info.time.Split(':')[0]);
                    int minute = Convert.ToInt32(org_info.time.Split(':')[1]);
                    org_info.date = new DateTime(org_info.date.Year, org_info.date.Month, org_info.date.Day, hour, minute, 0);


                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.organization_information.Add(org_info);
                    db.SaveChanges();

                    int first_remenu_id = db.ready_menu.ToList().FirstOrDefault().id;

                    Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("SiparisAl_Yemek", new { id = org_info.id + "_" + first_remenu_id });
                }
                else
                {
                    MLog.Error("Yeni Organizasyon Eklenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("OnKayit_EkleDuzenle");
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Yeni Organizasyon Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("OnKayit_EkleDuzenle");
            }
        }

        [HttpPost]
        public ActionResult Transition_SiparisDuzenle(FormCollection formCollection)
        {
            try
            {
                organization_information newOrgInfo = (organization_information)MTranslation.BuildObject(formCollection, "organization_information");
                if (newOrgInfo.organization_name != "" && newOrgInfo.name_surname != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    organization_information oldOrgInfo = db.organization_information.Where(w => w.id == newOrgInfo.id).FirstOrDefault();


                    // if people count has changed; all org-food(people_count and total_price) have to change
                    if (oldOrgInfo.people_count != newOrgInfo.people_count)
                    {
                        List<organization_and_food> orgAndFoods = db.organization_and_food.Where(w => w.organization_information_id == newOrgInfo.id).ToList();
                        foreach (organization_and_food orgAndFood in orgAndFoods)
                        {
                            orgAndFood.people_count = newOrgInfo.people_count;
                            orgAndFood.total_price = orgAndFood.price * newOrgInfo.people_count;
                        }
                        db.SaveChanges();
                    }


                    // update other changes
                    oldOrgInfo.people_count = newOrgInfo.people_count;
                    oldOrgInfo.date = newOrgInfo.date;
                    oldOrgInfo.down_payment = newOrgInfo.down_payment;
                    oldOrgInfo.email = newOrgInfo.email;
                    oldOrgInfo.name_surname = newOrgInfo.name_surname;
                    oldOrgInfo.organization_adress = newOrgInfo.organization_adress;
                    oldOrgInfo.organization_category_id = newOrgInfo.organization_category_id;
                    oldOrgInfo.organization_name = newOrgInfo.organization_name;
                    oldOrgInfo.organization_status = newOrgInfo.organization_status;
                    oldOrgInfo.organizators_adress = newOrgInfo.organizators_adress;
                    oldOrgInfo.referance = newOrgInfo.referance;
                    oldOrgInfo.telephone = newOrgInfo.telephone;
                    oldOrgInfo.telephone2 = newOrgInfo.telephone2;
                    oldOrgInfo.time = newOrgInfo.time;
                    oldOrgInfo.total_price = newOrgInfo.total_price;



                    // if user don't enter required object informations
                    oldOrgInfo.email = string.IsNullOrEmpty(oldOrgInfo.email) ? "belirtilmedi" : oldOrgInfo.email;
                    oldOrgInfo.organization_adress = string.IsNullOrEmpty(oldOrgInfo.organization_adress) ? "belirtilmedi" : oldOrgInfo.organization_adress;
                    oldOrgInfo.organizators_adress = string.IsNullOrEmpty(oldOrgInfo.organizators_adress) ? "belirtilmedi" : oldOrgInfo.organizators_adress;
                    oldOrgInfo.referance = string.IsNullOrEmpty(oldOrgInfo.referance) ? "belirtilmedi" : oldOrgInfo.referance;
                    oldOrgInfo.telephone = string.IsNullOrEmpty(oldOrgInfo.telephone) ? "00000000000" : oldOrgInfo.telephone;
                    oldOrgInfo.telephone2 = string.IsNullOrEmpty(oldOrgInfo.telephone2) ? "00000000000" : oldOrgInfo.telephone2;
                    oldOrgInfo.time = string.IsNullOrEmpty(oldOrgInfo.time) ? "belirtilmedi" : oldOrgInfo.time;
                    oldOrgInfo.date = (oldOrgInfo.date == null) ? DateTime.Now : oldOrgInfo.date;

                    // setting date
                    int hour = Convert.ToInt32(oldOrgInfo.time.Split(':')[0]);
                    int minute = Convert.ToInt32(oldOrgInfo.time.Split(':')[1]);
                    oldOrgInfo.date = new DateTime(oldOrgInfo.date.Year, oldOrgInfo.date.Month, oldOrgInfo.date.Day, hour, minute, 0);


                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON GÜNCELLENDİ.", Message_Type.Success);
                    return RedirectToAction("Siparis_Detay", new { id = newOrgInfo.id });
                }
                else
                {
                    MLog.Error("Organizasyon Düzenlenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("OnKayit_EkleDuzenle");
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Düzenlenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU: " + exception.Message, Message_Type.Error);

                return RedirectToAction("OnKayit_EkleDuzenle");
            }
        }

        public ActionResult Transition_SiparisSil(string id)
        {
            string redirect_url = "/Organizasyonlar/Listele";

            try
            {
                int org_info_id;

                if (id.StartsWith("arama"))
                {
                    org_info_id = Convert.ToInt32(id.Split('_')[1]);
                    redirect_url = "/Admin_Sistem/Arama";
                }
                else
                {
                    org_info_id = Convert.ToInt32(id);
                }


                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();
                if (org_info != null)
                {
                    List<organization_and_service> org_services = db.organization_and_service.Where(w => w.organization_information_id == org_info_id).ToList();
                    foreach (organization_and_service org_service in org_services)
                    {
                        db.organization_and_service.Remove(org_service);
                    }

                    List<organization_and_food> org_foods = db.organization_and_food.Where(w => w.organization_information_id == org_info_id).ToList();
                    foreach (organization_and_food org_service in org_foods)
                    {
                        db.organization_and_food.Remove(org_service);
                    }

                    db.organization_information.Remove(org_info);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON SİLİNDİ.", Message_Type.Success);
                    return Redirect(redirect_url);
                }
                else
                {
                    MLog.Error("Organizasyon Silinemedi", "Organizasyon Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "ORGANİZASYON BULUNMADI.", Message_Type.Error);

                    return Redirect(redirect_url);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return Redirect(redirect_url);
            }
        }

        public ActionResult Transition_SiparisServisSil(string id)
        {
            try
            {
                int service_id = Convert.ToInt32(id);
                int org_info_id;

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                organization_and_service org_and_service = db.organization_and_service.Where(w => w.id == service_id).FirstOrDefault();
                if (org_and_service != null)
                {
                    string service_name = org_and_service.organization_service.name;
                    org_info_id = org_and_service.organization_information_id;

                    db.organization_and_service.Remove(org_and_service);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", service_name + " SİLİNDİ.", Message_Type.Success);
                    return RedirectToAction("Siparis_Detay", new { id = org_info_id });
                }
                else
                {
                    MLog.Error("Organizasyondan Servis Silinemedi", "Servis Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "SERVİS BULUNMADI.", Message_Type.Error);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyondan Servis Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Index");
            }
        }


        public ActionResult Transition_OrganizasyonDurumunuBittiOlarakDegistir(string id)
        {
            try
            {
                int org_info_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();
                if (org_info != null)
                {
                    int finished_status = 2;

                    org_info.organization_status = finished_status;
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON BİTTİ OLARAK İŞARETLENDİ.", Message_Type.Success);
                    return Redirect("/Organizasyonlar/Listele");
                }
                else
                {
                    MLog.Error("Organizasyon Durumu Bitti Olarak Değiştirilemedi", "Organizasyon Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "ORGANİZASYON BULUNMADI.", Message_Type.Error);

                    return Redirect("/Organizasyonlar/Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Durumu Bitti Olarak Değiştirilemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return Redirect("/Organizasyonlar/Listele");
            }
        }

        public ActionResult OnKayit_Listele(string id)
        {
            int org_type = Convert.ToInt32(id);

            ViewData["org_type_id"] = org_type;

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_type> org_types = db.organization_type.ToList();
            ViewData["org_types"] = org_types;

            List<organization_information> org_infos = db.organization_information.ToList();

            List<organization_information> today_org_infos = new List<organization_information>();
            List<organization_information> future_org_infos = new List<organization_information>();

            List<organization_information> nonAccept_org_infos = new List<organization_information>();
            List<organization_information> finished_org_infos = new List<organization_information>();

            List<organization_information> completely_finished_org_infos = new List<organization_information>();


            List<organization_information> orgs = null;

            foreach (organization_information org_inf in org_infos)
            {
                if (org_inf.organization_status == 1) // Accepted org.s
                {
                    if (org_inf.date > DateTime.Now)
                    {
                        TimeSpan tsp = org_inf.date - DateTime.Now;
                        if (tsp.Days == 0)
                            today_org_infos.Add(org_inf);
                        if (tsp.Days > 0)
                            future_org_infos.Add(org_inf);
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
                else if (org_inf.organization_status == 2) // finished orgs
                {
                    completely_finished_org_infos.Add(org_inf);
                }
            }


            switch (org_type)
            {
                case 0:
                    orgs = today_org_infos.OrderBy(w => w.date).ToList();
                    break;
                case 1:
                    orgs = future_org_infos.OrderBy(w => w.date).ToList();
                    break;
                case 2:
                    orgs = finished_org_infos.OrderByDescending(w => w.date).ToList();
                    break;
                case 3:
                    orgs = nonAccept_org_infos.OrderBy(w => w.date).ToList();
                    break;
                case 4:
                    orgs = completely_finished_org_infos.OrderByDescending(w => w.date).ToList();
                    break;

                default:
                    break;
            }


            List<string> years = new List<string>();
            List<List<organization_information>> organizationsByYear = new List<List<organization_information>>();
            foreach (organization_information orgInfo in orgs)
            {
                string year = orgInfo.date.ToShortDateString().Split('.')[2];
                if (!years.Contains(year))
                {
                    years.Add(year);
                    organizationsByYear.Add(new List<organization_information>());
                }

                int index = years.IndexOf(year);
                organizationsByYear[index].Add(orgInfo);
            }

            ViewData["years"] = years;
            ViewData["organizationsByYear"] = organizationsByYear;

            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ORG. LİSTELE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        public ActionResult OnKayit_Listele_GET(string id)
        {
            string org_type = id;
            return RedirectToAction("OnKayit_Listele", new { id = org_type });
        }

        public ActionResult Istatistik()
        {

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_type> org_types = db.organization_type.ToList();
            ViewData["org_types"] = org_types;

            List<organization_information> org_infos = db.organization_information.ToList();

            List<organization_information> today_org_infos = new List<organization_information>();
            List<organization_information> future_org_infos = new List<organization_information>();

            List<organization_information> nonAccept_org_infos = new List<organization_information>();
            List<organization_information> finished_org_infos = new List<organization_information>();

            List<organization_information> completely_finished_org_infos = new List<organization_information>();


            foreach (organization_information org_inf in org_infos)
            {
                if (org_inf.organization_status == 1) // Accepted org.s
                {
                    if (org_inf.date > DateTime.Now)
                    {
                        TimeSpan tsp = org_inf.date - DateTime.Now;
                        if (tsp.Days == 0)
                            today_org_infos.Add(org_inf);
                        if (tsp.Days > 0)
                            future_org_infos.Add(org_inf);
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
                else if (org_inf.organization_status == 2) // finished orgs
                {
                    completely_finished_org_infos.Add(org_inf);
                }
            }



            List<string> years = new List<string>();
            foreach (organization_information orgInfo in org_infos)
            {
                string year = orgInfo.date.ToShortDateString().Split('.')[2];
                if (!years.Contains(year))
                {
                    years.Add(year);
                }
            }


            List<StatisticModel> statisticByYears = new List<StatisticModel>();
            foreach (string year in years)
            {
                StatisticModel statisticModel = new StatisticModel()
                {
                    year = year,
                    today_org_infos = new List<organization_information>(),
                    nonAccept_org_infos = new List<organization_information>(),
                    future_org_infos = new List<organization_information>(),
                    finished_org_infos = new List<organization_information>()
                };

                statisticByYears.Add(statisticModel);
            }


            // for sum
            StatisticModel sumStatisticModel = new StatisticModel()
            {
                year = "GENEL TOPLAM",
                today_org_infos = new List<organization_information>(),
                nonAccept_org_infos = new List<organization_information>(),
                future_org_infos = new List<organization_information>(),
                finished_org_infos = new List<organization_information>()
            };
            statisticByYears.Add(sumStatisticModel);

            // searching for today
            foreach (organization_information orgInfo in today_org_infos)
            {
                string year = orgInfo.date.ToShortDateString().Split('.')[2];
                StatisticModel statisticModel = statisticByYears.Where(w => w.year == year).FirstOrDefault();
                statisticModel.today_org_infos.Add(orgInfo);
                sumStatisticModel.today_org_infos.Add(orgInfo);
            }

            // searching for future
            foreach (organization_information orgInfo in future_org_infos)
            {
                string year = orgInfo.date.ToShortDateString().Split('.')[2];
                StatisticModel statisticModel = statisticByYears.Where(w => w.year == year).FirstOrDefault();
                statisticModel.future_org_infos.Add(orgInfo);
                sumStatisticModel.future_org_infos.Add(orgInfo);
            }

            // searching for nonAccepted
            foreach (organization_information orgInfo in nonAccept_org_infos)
            {
                string year = orgInfo.date.ToShortDateString().Split('.')[2];
                StatisticModel statisticModel = statisticByYears.Where(w => w.year == year).FirstOrDefault();
                statisticModel.nonAccept_org_infos.Add(orgInfo);
                sumStatisticModel.nonAccept_org_infos.Add(orgInfo);
            }

            // searching for finished
            foreach (organization_information orgInfo in finished_org_infos)
            {
                string year = orgInfo.date.ToShortDateString().Split('.')[2];
                StatisticModel statisticModel = statisticByYears.Where(w => w.year == year).FirstOrDefault();
                statisticModel.finished_org_infos.Add(orgInfo);
                sumStatisticModel.finished_org_infos.Add(orgInfo);
            }

            ViewData["statisticByYears"] = statisticByYears;


            return View();
        }



        [HttpPost]
        public ActionResult Transition_BilgilendirmeMailiGonder(FormCollection formCollection)
        {
            string str_org_info_id = formCollection["org_info_id"].ToString();
            int org_info_id = Convert.ToInt32(str_org_info_id);

            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();

                string email = org_info.email;

                if (email != null && email != "")
                {
                    // getting organization all information with admin level
                    string content = new ext_organization_information(org_info).Get_String_ExplainText("0", isForEmail: true);

                    string caption = "Akgül Catering - Organizasyon Bilgilendirme Dökümü";


                    bool result = MEmail.MailGonder(email, caption, content);
                    if (result)
                    {
                        Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON DÖKÜMÜ KULLANICIYA  E-MAIL GÖNDERİLDİ.", Message_Type.Success);
                    }
                    else
                    {
                        Session["message"] = new MessageModel("HATA", "E-MAİL GÖNDERİLEMEDİ.", Message_Type.Error);
                    }

                    return RedirectToAction("Siparis_Detay", new { id = org_info_id });
                }
                else
                {
                    MLog.Error("Organizasyon Dökümü Kullanıcıya E-MAIL Gönderilemedi", "Müşterinin Email'i Bulunamadı");
                    Session["message"] = new MessageModel("HATA", "MÜŞTERİNİN EMAİL'İ BULUNAMADI.", Message_Type.Error);

                    return RedirectToAction("Siparis_Detay", new { id = org_info_id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Dökümü Kullanıcıya E-MAIL Gönderilemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Siparis_Detay", new { id = org_info_id });
            }
        }

        public ActionResult SmsGonder(string id)
        {
            int org_info_id = Convert.ToInt32(id);

            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                organization_information org_info = db.organization_information.Where(w => w.id == org_info_id).FirstOrDefault();

                string _telephone = org_info.telephone;

                // getting organization all information with admin level
                string _content = new ext_organization_information(org_info).Get_String_ExplainText("0", "sms");

                SmsModel smsModel = new SmsModel()
                {
                    org_info_id = org_info_id,
                    phoneNumber = _telephone,
                    content = _content
                };
                ViewData["smsModel"] = smsModel;


                //Navigation______________________________________________________
                List<NavigationModel> navList = new List<NavigationModel>()
                {
                    new NavigationModel(){name="ORG. DETAY",url="/Admin_Sistem/Siparis_Detay/"+org_info_id},
                    new NavigationModel(){name="SMS GÖNDER",url=""},
                };
                ViewData["navigation"] = navList;
                //________________________________________________________________


                return View();
            }
            catch (Exception exception)
            {
                MLog.Error("SmsModel oluşturulamadı.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Siparis_Detay", new { id = org_info_id });
            }
        }

        [HttpPost]
        public ActionResult Transition_SmsGonder(FormCollection formCollection)
        {
            SmsModel smsModel = (SmsModel)MTranslation.BuildObject(formCollection, "SmsModel");

            try
            {
                if (smsModel.isValid())
                {

                    bool result = MSms.SmsGonder(smsModel);
                    if (result)
                    {
                        Session["message"] = new MessageModel("Bilgi", "ORGANİZASYON DÖKÜMÜ SMS GÖNDERİLDİ.", Message_Type.Success);
                    }
                    else
                    {
                        Session["message"] = new MessageModel("HATA", "SMS GÖNDERİLEMEDİ.", Message_Type.Error);
                    }

                    return RedirectToAction("Siparis_Detay", new { id = smsModel.org_info_id });
                }
                else
                {
                    MLog.Error("Organizasyon Dökümü Kullanıcıya SMS Gönderilemedi", "Telefon ve/veya İçerik Hatalı");
                    Session["message"] = new MessageModel("HATA", "MÜŞTERİNİN TELEFONU BULUNAMADI.", Message_Type.Error);

                    return RedirectToAction("Siparis_Detay", new { id = smsModel.org_info_id });
                }

            }
            catch (Exception exception)
            {
                MLog.Error("Organizasyon Dökümü Kullanıcıya SMS Gönderilemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Siparis_Detay", new { id = smsModel.org_info_id });
            }
        }



        #endregion


        #region ARAMA

        public ActionResult Arama()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_type> org_types = db.organization_type.ToList();
            ViewData["org_types"] = org_types;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="ARAMA",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }
        public ActionResult Transition_Arama(FormCollection formCollection)
        {
            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                List<organization_information> org_infos = db.organization_information.ToList();
                List<organization_information> _return_org = new List<organization_information>();

                if (formCollection["with_name"] != null || formCollection["with_phone"] != null)
                {
                    if (formCollection["with_name"] != null)
                    {
                        string name = formCollection["with_name"].ToString();
                        _return_org.AddRange(org_infos.Where(w => w.name_surname.ToLower().Contains(name.ToLower())).ToList());
                    }
                    else if (formCollection["with_phone"] != null)
                    {
                        string phone = formCollection["with_phone"].ToString();
                        _return_org.AddRange(org_infos.Where(w => w.telephone.ToLower().Contains(phone.ToLower())).ToList());
                        _return_org.AddRange(org_infos.Where(w => w.telephone2.ToLower().Contains(phone.ToLower())).ToList());
                    }

                    TempData["org_infos"] = _return_org;
                    return RedirectToAction("Arama");
                }
                else
                {
                    MLog.Error("Organiasyonlarda Arama İşlemi Yapılamadı", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "ARAMA YAPMAK İÇİN İSİM VEYA TELEFON NUMARASI GİRİN.", Message_Type.Error);

                    return RedirectToAction("Arama");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Organiasyonlarda Arama İşlemi Yapılamadı", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Arama");
            }
        }

        #endregion


        #region Kullanici

        public ActionResult Kullanici_Listele(string id)
        {
            ViewData["id"] = id;

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
            List<user> users = db.user.ToList();
            ViewData["users"] = users;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="KULLANICI LİSTELE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        public ActionResult Kullanici_EkleDuzenle(string id)
        {
            user _user = null;

            if (id != null)
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                string telephone = id.StartsWith("0") ? id : "0" + id;
                _user = db.user.Where(w => w.telephone == telephone).FirstOrDefault();
            }

            ViewData["user"] = _user;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="KULLANICI LİSTELE",url="/Admin_Sistem/Kullanici_Listele"},
                new NavigationModel(){name="KULLANICI EKLE-DÜZENLE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        [HttpPost]
        public ActionResult Transition_KullaniciEkle(FormCollection formCollection)
        {
            try
            {
                user _user = (user)MTranslation.BuildObject(formCollection, "user");
                _user.telephone = _user.telephone.StartsWith("0") ? _user.telephone : "0" + _user.telephone;

                if (_user.telephone != "" && _user.password != "" && _user.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    db.user.Add(_user);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "KULLANICI EKLENDİ.", Message_Type.Success);
                    return RedirectToAction("Kullanici_Listele");
                }
                else
                {
                    MLog.Error("Yeni Kullanıcı Eklenemedi", "Kullanıcı Eklenemedi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİ GİRİLDİ.", Message_Type.Error);

                    return RedirectToAction("Kullanici_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Yeni Kullanıcı Eklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Kullanici_Listele");
            }
        }

        [HttpPost]
        public ActionResult Transition_KullaniciDuzenle(FormCollection formCollection)
        {
            try
            {
                user _user = (user)MTranslation.BuildObject(formCollection, "user");
                _user.telephone = _user.telephone.StartsWith("0") ? _user.telephone : "0" + _user.telephone;

                if (_user.password != "" && _user.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    user mapped_user = db.user.Where(w => w.telephone == _user.telephone).FirstOrDefault();

                    if (mapped_user != null)
                    {
                        mapped_user.job = _user.job;
                        mapped_user.name = _user.name;
                        mapped_user.type = _user.type;
                        mapped_user.password = _user.password;


                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", "KULLANICI DÜZENLENDİ.", Message_Type.Success);
                        return RedirectToAction("Kullanici_Listele");
                    }
                    else
                    {
                        MLog.Error("Kullanıcı Düzenlenemedi", "Kullanıcı Bulunamadı");
                        Session["message"] = new MessageModel("HATA", "KULLANICI BULUNAMADI.", Message_Type.Error);

                        return RedirectToAction("Kullanici_Listele");
                    }
                }
                else
                {
                    MLog.Error("Kullanıcı Düzenlenemedi", "Eksik Bilgi Girildi");
                    Session["message"] = new MessageModel("HATA", "EKSİK BİLGİLER VAR.", Message_Type.Error);

                    return RedirectToAction("Kullanici_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Kullanıcı Düzenlenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Kullanici_Listele");
            }
        }


        public ActionResult Transition_KullaniciSil(string id)
        {
            try
            {
                if (id != null)
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    string telephone = id.StartsWith("0") ? id : "0" + id;
                    user _user = db.user.Where(w => w.telephone == telephone).FirstOrDefault();
                    if (_user != null)
                    {
                        db.user.Remove(_user);
                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", "KULLANICI SİLİNDİ.", Message_Type.Success);
                        return RedirectToAction("Kullanici_Listele");
                    }
                    else
                    {
                        MLog.Error("Kullanıcı Silinemedi", "Kullanıcı Bulunamadı");
                        Session["message"] = new MessageModel("HATA", "BÖYLE BİR KULLANICI YOK.", Message_Type.Error);

                        return RedirectToAction("Kullanici_Listele");
                    }
                }
                else
                {
                    MLog.Error("Kullanıcı Silinemedi", "Parametre Hatalı");
                    Session["message"] = new MessageModel("HATA", "PARAMETRE HATALI.", Message_Type.Error);

                    return RedirectToAction("Kullanici_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Kullanıcı Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);

                return RedirectToAction("Kullanici_Listele");
            }
        }

        #endregion


        #region Rapor

        public ActionResult RaporAl()
        {
            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>()
            {
                new NavigationModel(){name="RAPOR AL",url=""},
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________

            return View();
        }

        public ActionResult Transition_RaporAl(FormCollection formCollection)
        {
            ReportModel reportModel = (ReportModel)MTranslation.BuildObject(formCollection, "ReportModel");

            switch (reportModel.reportType)
            {
                case "organizasyonlarDetay":
                    return Transition_RaporAl_DetayliOrganizasyonRaporu(reportModel);

                case "personelSayisi":
                    return Transition_RaporAl_PersonelRaporu(reportModel);

                case "yemekMiktarlari":
                    return Transition_RaporAl_YemekMiktarlari(reportModel);

                case "organizasyonlarOdeme":
                    return Transition_RaporAl_OrganizasyonOdemeRaporu(reportModel);

                case "emanetler":
                    return Transition_RaporAl_EmanetlerRaporu(reportModel);

                default:
                    break;
            }

            return View();
        }


        //-----------------------------------------------------------------------------------


        public ActionResult Transition_RaporAl_PersonelRaporu(ReportModel reportModel)
        {
            try
            {
                // get the list of organization whichs are between the selected dates
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                List<organization_information> orgInfos = db.organization_information.Where(w => w.date >= reportModel.startDate && w.date <= reportModel.stopDate).OrderBy(w => w.date).ToList();

                //----------------------------------------MExcell Operations Start------------------------------------------------
                // create an MExcell instance
                MExcell mexcell = new MExcell();

                // defining the columns
                mexcell.AddColumn(name: "NO", width: 4);
                mexcell.AddColumn(name: "ORGANİZASYON ADI", width: 30);
                mexcell.AddColumn(name: "AD SOYAD", width: 15);
                mexcell.AddColumn(name: "TARİH", width: 10);
                mexcell.AddColumn(name: "SAAT", width: 8);
                mexcell.AddColumn(name: "AŞÇI", width: 8);
                mexcell.AddColumn(name: "ŞEF", width: 8);
                mexcell.AddColumn(name: "GARSON", width: 8);
                mexcell.AddColumn(name: "DİĞER", width: 8);

                // writing rows
                for (int i = 0; i < orgInfos.Count; i++)
                {
                    mexcell.PassNextLine();

                    organization_information orgInfo = orgInfos.ElementAt(i);

                    mexcell.WriteSpecificAdress(1, (i + 1).ToString());
                    mexcell.WriteSpecificAdress(2, orgInfo.organization_name);
                    mexcell.WriteSpecificAdress(3, orgInfo.name_surname);
                    mexcell.WriteSpecificAdress(4, orgInfo.date.ToShortDateString());
                    mexcell.WriteSpecificAdress(5, orgInfo.time);
                    mexcell.WriteSpecificAdress(6, orgInfo.count_cook.ToString());
                    mexcell.WriteSpecificAdress(7, orgInfo.count_chef.ToString());
                    mexcell.WriteSpecificAdress(8, orgInfo.count_waiter.ToString());
                    mexcell.WriteSpecificAdress(9, orgInfo.count_other.ToString());
                }

                // saving excell file:
                // delete old files on the exports directory
                string filePath = Server.MapPath("/Exports");
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                    System.IO.File.Delete(file);

                string fileName = "PersonelRaporu_" + reportModel.startDate.ToShortDateString() + "-" + reportModel.stopDate.ToShortDateString() + ".xlsx";
                bool result = mexcell.WriteToFile(filePath, fileName);
                //----------------------------------------MExcell Operations End------------------------------------------------

                if (result)
                {
                    // Session["message"] = new MessageModel("Bilgi", "Rapor Alma İşlemi Başarılı.", Message_Type.Success);
                    Response.Redirect("/Exports/" + fileName);
                }
                else
                {
                    MLog.Error("Rapor Alma İşlemi Başarısız", "Rapor Alınamadı");
                    Session["message"] = new MessageModel("Rapor Alma İşlemi Başarısız", "Bir şeyler ters gitti.", Message_Type.Error);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Rapor Alma İşlemi Başarısız", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);
            }

            return RedirectToAction("RaporAl");
        }

        public ActionResult Transition_RaporAl_YemekMiktarlari(ReportModel reportModel)
        {
            try
            {
                // get the list of organization whichs are between the selected dates
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                List<organization_information> orgInfos = db.organization_information.Where(w => w.date >= reportModel.startDate && w.date <= reportModel.stopDate).OrderBy(w => w.date).ToList();

                List<food> foods = db.food.ToList();

                //----------------------------------------MExcell Operations Start------------------------------------------------
                // create an MExcell instance
                MExcell mexcell = new MExcell();

                // defining the columns
                mexcell.AddColumn(name: "NO", width: 4);
                mexcell.AddColumn(name: "ORGANİZASYON ADI", width: 30);
                mexcell.AddColumn(name: "AD SOYAD", width: 15);
                mexcell.AddColumn(name: "TARİH", width: 10);
                mexcell.AddColumn(name: "SAAT", width: 8);
                mexcell.AddColumn(name: "KİŞİ SAYISI", width: 8);

                // this indicates the column which is not food
                int shiftColumnCount = 6;

                //write all foods as a column on the report
                for (int i = 0; i < foods.Count; i++)
                {
                    food _food = foods.ElementAt(i);
                    mexcell.AddColumn(name: _food.name + " (" + _food.food_units.name + ")", width: 27);
                }


                // writing rows
                for (int i = 0; i < orgInfos.Count; i++)
                {
                    organization_information orgInfo = orgInfos.ElementAt(i);

                    List<organization_and_food> orgsFood = db.organization_and_food.Where(w => w.organization_information_id == orgInfo.id).ToList();


                    mexcell.PassNextLine();



                    mexcell.WriteSpecificAdress(1, (i + 1).ToString());
                    mexcell.WriteSpecificAdress(2, orgInfo.organization_name);
                    mexcell.WriteSpecificAdress(3, orgInfo.name_surname);
                    mexcell.WriteSpecificAdress(4, orgInfo.date.ToShortDateString());
                    mexcell.WriteSpecificAdress(5, orgInfo.time);
                    mexcell.WriteSpecificAdress(6, orgInfo.people_count.ToString());
                    for (int j = 0; j < orgsFood.Count; j++)
                    {
                        organization_and_food orgAndFood = orgsFood.ElementAt(j);
                        food oFood = foods.Where(w => w.id == orgAndFood.food_id).FirstOrDefault();

                        int index = foods.IndexOf(oFood);
                        int columnIndex = 1 + shiftColumnCount + index;
                        mexcell.WriteSpecificAdress(columnIndex, (orgAndFood.quantity * orgAndFood.people_count).ToString());
                    }
                }

                int rowIndex = 1 + orgInfos.Count;
                mexcell.PassNextLine();
                mexcell.MergeTwoCells(1, shiftColumnCount);
                mexcell.AddColumn(name: "Toplam");
                string[] ColumnNames = { "0", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ", "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ", "GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ", "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ" };
                for (int j = 0; j < foods.Count; j++)
                {
                    food oFood = foods.ElementAt(j);

                    int columnIndex = 1 + shiftColumnCount + j;

                    string formulaColumnChar = ColumnNames[columnIndex];
                    string formula = "SUM(" + formulaColumnChar + "2" + ":" + formulaColumnChar + rowIndex + ")";
                    double calculatedFormula = mexcell.EvaluateFormula(formula);
                    string calculatedString = "";
                    if (oFood.food_units.name == "Gram" && calculatedFormula > 1000)
                    {
                        calculatedFormula /= 1000;
                        calculatedString = calculatedFormula + " KG";
                    }
                    else
                    {
                        calculatedString = calculatedFormula + " " + oFood.food_units.name;
                    }

                    string value = oFood.name + ": " + calculatedString;
                    //mexcell.WriteSpecificAdress(columnIndex, calculatedFormula);
                    mexcell.AddColumn(columnIndex, value);
                }

                // saving excell file:
                // delete old files on the exports directory
                string filePath = Server.MapPath("/Exports");
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                    System.IO.File.Delete(file);

                string fileName = "YemekMiktariRaporu_" + reportModel.startDate.ToShortDateString() + "-" + reportModel.stopDate.ToShortDateString() + ".xlsx";
                bool result = mexcell.WriteToFile(filePath, fileName);
                //----------------------------------------MExcell Operations End------------------------------------------------

                if (result)
                {
                    // Session["message"] = new MessageModel("Bilgi", "Rapor Alma İşlemi Başarılı.", Message_Type.Success);
                    Response.Redirect("/Exports/" + fileName);
                }
                else
                {
                    MLog.Error("YemekMiktariRaporu Rapor Alma İşlemi Başarısız", "Rapor Alınamadı");
                    Session["message"] = new MessageModel("Rapor Alma İşlemi Başarısız", "Bir şeyler ters gitti.", Message_Type.Error);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("YemekMiktariRaporu Rapor Alma İşlemi Başarısız", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);
            }

            return RedirectToAction("RaporAl");
        }

        public ActionResult Transition_RaporAl_DetayliOrganizasyonRaporu(ReportModel reportModel)
        {
            try
            {
                // get the list of organization whichs are between the selected dates
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                List<organization_information> orgInfos = db.organization_information.Where(w => w.date >= reportModel.startDate && w.date <= reportModel.stopDate && w.organization_status == 1).OrderBy(w => w.date).ToList();

                //----------------------------------------MExcell Operations Start------------------------------------------------
                // create an MExcell instance
                MExcell mexcell = new MExcell();


                for (int i = 0; i < orgInfos.Count; i++)
                {
                    organization_information orgInfo = orgInfos.ElementAt(i);

                    // for drawing box on excell document
                    mexcell.SetStartPoint();

                    // defining the columns
                    mexcell.AddColumn(name: "NO", width: 10);
                    mexcell.AddColumn(name: "ORGANİZASYON ADI", width: 25);
                    mexcell.AddColumn(name: "AD SOYAD - Telefon", width: 15);
                    mexcell.AddColumn(name: "TARİH", width: 10);
                    mexcell.AddColumn(name: "SAAT", width: 10);
                    mexcell.AddColumn(name: "KİŞİ SAYISI", width: 10);

                    mexcell.PassNextLine();



                    // writing rows
                    mexcell.WriteSpecificAdress(1, (i + 1).ToString());
                    mexcell.WriteSpecificAdress(2, orgInfo.organization_name);
                    mexcell.WriteSpecificAdress(3, orgInfo.name_surname + " - " + orgInfo.telephone);
                    mexcell.WriteSpecificAdress(4, orgInfo.date.ToShortDateString());
                    mexcell.WriteSpecificAdress(5, orgInfo.time);
                    mexcell.WriteSpecificAdress(6, orgInfo.people_count.ToString());

                    mexcell.WriteBlankLine();

                    //--------------------------------------------------------------

                    List<organization_service> base_services = db.organization_service.ToList();
                    List<organization_and_food> foods = db.organization_and_food.Where(w => w.organization_information_id == orgInfo.id).ToList();
                    List<organization_and_service> services = db.organization_and_service.Where(w => w.organization_information_id == orgInfo.id).ToList();

                    float calculated_price = 0;


                    mexcell.AddColumn(j: 1, name: "YEMEK ADI");
                    mexcell.MergeTwoCells(1, 2);
                    mexcell.AddColumn(j: 3, name: "MİKTAR");
                    mexcell.AddColumn(j: 4, name: "BİRİM");
                    mexcell.AddColumn(j: 5, name: "KİŞİ SAYISI");
                    mexcell.AddColumn(j: 6, name: "FİYAT");


                    foreach (organization_and_food org_food in foods)
                    {
                        calculated_price += org_food.total_price;

                        mexcell.PassNextLine();

                        mexcell.WriteSpecificAdress(1, org_food.food.name);
                        mexcell.MergeTwoCells(1, 2);
                        mexcell.WriteSpecificAdress(3, org_food.quantity.ToString());
                        mexcell.WriteSpecificAdress(4, org_food.food.food_units.name);
                        mexcell.WriteSpecificAdress(5, org_food.people_count + " Kişi");
                        mexcell.WriteSpecificAdress(6, org_food.total_price + " TL");
                    }

                    mexcell.WriteBlankLine();


                    // SERVICES
                    mexcell.AddColumn(j: 1, name: "ADI");
                    mexcell.AddColumn(j: 2, name: "İÇERİK");
                    mexcell.MergeTwoCells(2, 3);
                    mexcell.AddColumn(j: 4, name: "FİYAT");


                    foreach (organization_service org_base_serv in base_services)
                    {
                        organization_and_service org_service = services.Where(w => w.organization_service_id == org_base_serv.id).FirstOrDefault();

                        if (org_service == null)
                        {
                            continue;
                        }

                        calculated_price += org_service.price;

                        mexcell.PassNextLine();

                        mexcell.WriteSpecificAdress(1, org_service.organization_service.name);
                        mexcell.WriteSpecificAdress(2, org_service.information);
                        mexcell.MergeTwoCells(2, 3);
                        mexcell.WriteSpecificAdress(4, org_service.price + " TL");
                    }

                    calculated_price = calculated_price - orgInfo.down_payment - orgInfo.discount;

                    mexcell.WriteBlankLine();

                    mexcell.AddColumn(j: 1, name: "KAPORA");
                    mexcell.AddColumn(j: 2, name: "İNDİRİM");
                    mexcell.AddColumn(j: 3, name: "TOPLAM FİYAT");

                    mexcell.PassNextLine();

                    mexcell.WriteSpecificAdress(1, orgInfo.down_payment.ToString());
                    mexcell.WriteSpecificAdress(2, orgInfo.discount.ToString());
                    mexcell.WriteSpecificAdress(3, calculated_price + " TL");


                    mexcell.SetStopPointAndDrawBorder(6);

                    mexcell.WriteBlankLine();
                    mexcell.WriteBlankLine();



                    // başlık,içerik,boşluk,başlık,foodcount,boşluk,başlık,içerik
                    //int rowCount = 1 + 1 + 1 + 1 + foods.Count + 1 + 1 + base_services.Count + 1 + 1 + 1;
                    //mexcell.MakeBordered(0, 0, 0, 0);
                }


                //--------------------------------------------------------------


                // saving excell file:
                // delete old files on the exports directory
                string filePath = Server.MapPath("/Exports");
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                    System.IO.File.Delete(file);

                string fileName = "OrganizasyonRaporu_" + reportModel.startDate.ToShortDateString() + "-" + reportModel.stopDate.ToShortDateString() + ".xlsx";
                bool result = mexcell.WriteToFile(filePath, fileName);
                //----------------------------------------MExcell Operations End------------------------------------------------

                if (result)
                {
                    // Session["message"] = new MessageModel("Bilgi", "Rapor Alma İşlemi Başarılı.", Message_Type.Success);
                    Response.Redirect("/Exports/" + fileName);
                }
                else
                {
                    MLog.Error("Rapor Alma İşlemi Başarısız", "Rapor Alınamadı");
                    Session["message"] = new MessageModel("Rapor Alma İşlemi Başarısız", "Bir şeyler ters gitti.", Message_Type.Error);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Rapor Alma İşlemi Başarısız", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);
            }

            return RedirectToAction("RaporAl");
        }

        public ActionResult Transition_RaporAl_OrganizasyonOdemeRaporu(ReportModel reportModel)
        {
            try
            {
                // get the list of organization whichs are between the selected dates
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                List<organization_information> orgInfos = db.organization_information.Where(w => w.date >= reportModel.startDate && w.date <= reportModel.stopDate && w.organization_status == 1).OrderBy(w => w.date).ToList();

                //----------------------------------------MExcell Operations Start------------------------------------------------
                // create an MExcell instance
                MExcell mexcell = new MExcell();

                // defining the columns
                mexcell.AddColumn(name: "NO", width: 10);
                mexcell.AddColumn(name: "ORGANİZASYON ADI", width: 25);
                mexcell.AddColumn(name: "TOPLAM FİYAT", width: 15);
                mexcell.AddColumn(name: "KAPORA", width: 10);
                mexcell.AddColumn(name: "İNDİRİM", width: 10);
                mexcell.AddColumn(name: "KALAN", width: 10);

                mexcell.PassNextLine();


                for (int i = 0; i < orgInfos.Count; i++)
                {
                    organization_information orgInfo = orgInfos.ElementAt(i);

                    List<organization_service> base_services = db.organization_service.ToList();
                    List<organization_and_food> foods = db.organization_and_food.Where(w => w.organization_information_id == orgInfo.id).ToList();
                    List<organization_and_service> services = db.organization_and_service.Where(w => w.organization_information_id == orgInfo.id).ToList();

                    float calculated_price = 0;

                    foreach (organization_and_food org_food in foods)
                    {
                        calculated_price += org_food.total_price;
                    }


                    foreach (organization_service org_base_serv in base_services)
                    {
                        organization_and_service org_service = services.Where(w => w.organization_service_id == org_base_serv.id).FirstOrDefault();

                        if (org_service == null)
                            continue;

                        calculated_price += org_service.price;
                    }

                    float calculated_priceWithPayments = calculated_price - orgInfo.down_payment - orgInfo.discount;

                    // writing rows
                    mexcell.WriteSpecificAdress(1, (i + 1).ToString(), true);
                    mexcell.WriteSpecificAdress(2, orgInfo.organization_name);
                    mexcell.WriteSpecificAdress(3, calculated_price.ToString(), true);
                    mexcell.WriteSpecificAdress(4, orgInfo.down_payment.ToString(), true);
                    mexcell.WriteSpecificAdress(5, orgInfo.discount.ToString(), true);
                    mexcell.WriteSpecificAdress(6, calculated_priceWithPayments.ToString(), true);

                    mexcell.PassNextLine();
                }


                //--------------------------------------------------------------


                // saving excell file:
                // delete old files on the exports directory
                string filePath = Server.MapPath("/Exports");
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                    System.IO.File.Delete(file);

                string fileName = "OrganizasyonOdemeRaporu_" + reportModel.startDate.ToShortDateString() + "-" + reportModel.stopDate.ToShortDateString() + ".xlsx";
                bool result = mexcell.WriteToFile(filePath, fileName);
                //----------------------------------------MExcell Operations End------------------------------------------------

                if (result)
                {
                    // Session["message"] = new MessageModel("Bilgi", "Rapor Alma İşlemi Başarılı.", Message_Type.Success);
                    Response.Redirect("/Exports/" + fileName);
                }
                else
                {
                    MLog.Error("Rapor Alma İşlemi Başarısız", "Rapor Alınamadı");
                    Session["message"] = new MessageModel("Rapor Alma İşlemi Başarısız", "Bir şeyler ters gitti.", Message_Type.Error);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Rapor Alma İşlemi Başarısız", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);
            }

            return RedirectToAction("RaporAl");
        }

        public ActionResult Transition_RaporAl_EmanetlerRaporu(ReportModel reportModel)
        {
            try
            {
                // get the list of organization whichs are between the selected dates
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                List<organization_information> orgInfos = db.organization_information.Where(w => w.date >= reportModel.startDate && w.date <= reportModel.stopDate).OrderBy(w => w.date).ToList();

                //----------------------------------------MExcell Operations Start------------------------------------------------
                // create an MExcell instance
                MExcell mexcell = new MExcell();

                // defining the columns
                mexcell.AddColumn(name: "NO", width: 4);
                mexcell.AddColumn(name: "ORGANİZASYON ADI", width: 30);
                mexcell.AddColumn(name: "TARİH", width: 30);
                mexcell.AddColumn(name: "SAAT", width: 30);
                mexcell.AddColumn(name: "EMANET ADI", width: 15);
                mexcell.PassNextLine();

                // writing rows
                for (int i = 0; i < orgInfos.Count; i++)
                {
                    organization_information orgInfo = orgInfos.ElementAt(i);

                    List<organization_and_service> org_and_services = db.organization_and_service.Where(w => w.organization_information_id == orgInfo.id).ToList();
                    if (org_and_services.Count <= 0)
                        continue;

                    for (int j = 0; j < org_and_services.Count; j++)
                    {
                        organization_and_service orgService = org_and_services[j];
                        if (orgService.organization_service.name == "Emanet")
                        {
                            mexcell.WriteSpecificAdress(1, (i + 1).ToString());
                            mexcell.WriteSpecificAdress(2, orgInfo.organization_name);
                            mexcell.WriteSpecificAdress(3, orgInfo.date.ToShortDateString());
                            mexcell.WriteSpecificAdress(4, orgInfo.time);
                            mexcell.WriteSpecificAdress(5, orgService.information);
                            mexcell.PassNextLine();
                        }
                    }
                }

                // saving excell file:
                // delete old files on the exports directory
                string filePath = Server.MapPath("/Exports");
                string[] files = Directory.GetFiles(filePath);
                foreach (string file in files)
                    System.IO.File.Delete(file);

                string fileName = "EmanetlerRaporu_" + reportModel.startDate.ToShortDateString() + "-" + reportModel.stopDate.ToShortDateString() + ".xlsx";
                bool result = mexcell.WriteToFile(filePath, fileName);
                //----------------------------------------MExcell Operations End------------------------------------------------

                if (result)
                {
                    // Session["message"] = new MessageModel("Bilgi", "Rapor Alma İşlemi Başarılı.", Message_Type.Success);
                    Response.Redirect("/Exports/" + fileName);
                }
                else
                {
                    MLog.Error("Rapor Alma İşlemi Başarısız", "Rapor Alınamadı");
                    Session["message"] = new MessageModel("Rapor Alma İşlemi Başarısız", "Bir şeyler ters gitti.", Message_Type.Error);
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Rapor Alma İşlemi Başarısız", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "BİR HATA OLUŞTU.", Message_Type.Error);
            }

            return RedirectToAction("RaporAl");
        }


        #endregion

    }
}
