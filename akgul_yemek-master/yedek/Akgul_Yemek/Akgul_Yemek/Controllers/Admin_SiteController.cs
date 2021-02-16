using Akgul_Yemek.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akgul_Yemek.Code;

namespace Akgul_Yemek.Controllers
{
    public class Admin_SiteController : Controller
    {
        //
        // GET: /Admin_Site/

        #region STATISTICS

        public ActionResult Index()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            // assing statistics
            Site_Istatistik_Model admin_istatistik = new Site_Istatistik_Model()
            {
                username = Session["name"].ToString(),
                food_count = db.site_food_menu.Count(),
                gallery_count = db.gallery_titles.Count(),
                menu_count = db.site_menu.Count(),
                slider_count = db.slider.Count()
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


        #region SLIDER

        public ActionResult Slider()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
            List<slider> sliders = db.slider.ToList();
            ViewData["sliders"] = sliders;

            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="SLIDER",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        [HttpPost]
        public ActionResult Transition_SliderEkle(FormCollection formCollection)
        {
            int random_number = new Random().Next(0, 1024);

            try
            {
                slider slide = (slider)MTranslation.BuildObject(formCollection, "slider");

                // adding picture to Disk
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Files/Slider"), "" + random_number + filename);
                    file.SaveAs(path);

                    // food image_name is uploaded picture name
                    slide.filename = "" + random_number + filename;
                    slide.keywords = "akgül yemek,akgül organizasyon,akgül";

                    // adding food to Database
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                    db.slider.Add(slide);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Slayt Resmi Ekendi.", Message_Type.Success);
                    return RedirectToAction("Slider");

                }
                else
                {
                    MLog.Error("Slayt Resmi Ekenemedi.", "Slider Fotoğraf Yüklenemedi");
                    Session["message"] = new MessageModel("HATA", "Slider Fotoğraf Yüklenemedi", Message_Type.Error);

                    return RedirectToAction("Slider");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Slider Fotoğraf Yüklenemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Slider");
            }
        }

        public ActionResult Transition_SliderSil(string id)
        {
            try
            {
                int slide_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                slider slide = db.slider.Where(w => w.id == slide_id).FirstOrDefault();
                if (slide != null)
                {
                    //deleting picture from disk
                    string file_path = Path.Combine(Server.MapPath("~/Files/Slider"), slide.filename);
                    if (System.IO.File.Exists(file_path))
                        System.IO.File.Delete(file_path);

                    // deleting from database
                    db.slider.Remove(slide);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Slayt Resmi Silindi.", Message_Type.Success);
                    return RedirectToAction("Slider");
                }
                else
                {
                    MLog.Error("Slayf Resmi Silinemedi.", "Belirtilen Resim Bulunamadı.");
                    Session["message"] = new MessageModel("Slayt Resmi Silinemedi", "Belirtilen Resim Bulunamadı.", Message_Type.Error);

                    return RedirectToAction("Slider");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Slayt Resmi Silinemedi", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Slider");
            }
        }


        #endregion


        #region MENU

        public ActionResult Menu_Listele()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<site_menu> site_menu_s = db.site_menu.ToList();
            ViewData["site_menu_s"] = site_menu_s;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="MENÜLER",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }


        public ActionResult Menu_Detay(string id)
        {
            int menu_id = Convert.ToInt32(id);

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            site_menu _site_menu = db.site_menu.Where(w => w.id == menu_id).FirstOrDefault();
            ViewData["site_menu"] = _site_menu;

            List<site_food_menu> site_foods = db.site_food_menu.Where(w => w.site_menu_id == menu_id).ToList();
            ViewData["site_foods"] = site_foods;



            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="MENÜLER",url="/Admin_Site/Menu_Listele"},
                new NavigationModel(){name="MENÜ DETAY",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        [HttpPost]
        public ActionResult Transition_MenuEkle(FormCollection formCollection)
        {
            try
            {
                site_menu _site_menu = (site_menu)MTranslation.BuildObject(formCollection, "site_menu");

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                if (_site_menu.name != "")
                {
                    db.site_menu.Add(_site_menu);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Menü Ekendi.", Message_Type.Success);
                    return RedirectToAction("Menu_Listele");
                }
                else
                {
                    Session["message"] = new MessageModel("Menü Eklenemedi.", "Eksik Bilgi Girildi.", Message_Type.Error);
                    MLog.Error("Menü Eklenemedi.", "Eksik Bilgi Girildi.");

                    return RedirectToAction("Menu_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Menü Ekenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Menu_Listele");
            }
        }


        public ActionResult Transition_MenuSil(string id)
        {
            try
            {
                int menu_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                site_menu _site_menu = db.site_menu.Where(w => w.id == menu_id).FirstOrDefault();
                if (_site_menu != null)
                {
                    db.site_menu.Remove(_site_menu);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Menü Silindi.", Message_Type.Success);
                    return RedirectToAction("Menu_Listele");
                }
                else
                {
                    MLog.Error("Menü Silinemedi.", "Belirtilen Menu Bulunamadı.");
                    Session["message"] = new MessageModel("HATA", "Belirtilen Menu Bulunamadı.", Message_Type.Error);

                    return RedirectToAction("Menu_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Menü Silinemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Menu_Listele");
            }
        }


        [HttpPost]
        public ActionResult Transition_MenuYemekEkle(FormCollection formCollection)
        {
            int random_number = new Random().Next(0, 1024);
            int menu_id = 0;

            try
            {
                site_food_menu site_food = (site_food_menu)MTranslation.BuildObject(formCollection, "site_food_menu");
                menu_id = site_food.site_menu_id;

                // adding picture to Disk
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Files/Food_Menu"), "" + random_number + filename);
                    file.SaveAs(path);

                    // food image_name is uploaded picture name
                    site_food.image_name = "" + random_number + filename;

                    // adding food to Database
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    if (site_food.name != "")
                    {
                        db.site_food_menu.Add(site_food);
                        db.SaveChanges();

                        Session["message"] = new MessageModel("Bilgi", "Yemek Menüye Ekendi.", Message_Type.Success);
                        return RedirectToAction("Menu_Detay", new { id = "" + menu_id });
                    }
                    else
                    {
                        MLog.Error("Yemek Menüye Ekenemedi.", "Eksik Bilgi Girildi.");
                        Session["message"] = new MessageModel("HATA", "Eksik Bilgi Girildi.", Message_Type.Error);

                        return RedirectToAction("Menu_Detay", new { id = "" + menu_id });
                    }
                }
                else
                {
                    MLog.Error("Yemek Menüye Ekenemedi.", "Eksik Bilgi Girildi.");
                    Session["message"] = new MessageModel("HATA", "Eksik Bilgi Girildi.", Message_Type.Error);

                    return RedirectToAction("Menu_Detay", new { id = "" + menu_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Yemek Menüye Ekenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Menu_Detay", new { id = "" + menu_id });
            }
        }


        public ActionResult Transition_MenuYemekSil(string id)
        {
            int menu_id = 0;

            try
            {
                int yemek_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                site_food_menu site_food = db.site_food_menu.Where(w => w.id == yemek_id).FirstOrDefault();
                if (site_food != null)
                {
                    menu_id = site_food.site_menu_id;


                    //deleting picture from disk
                    string file_path = Path.Combine(Server.MapPath("~/Files/Food_Menu"), site_food.image_name);
                    if (System.IO.File.Exists(file_path))
                        System.IO.File.Delete(file_path);

                    // deleting from database
                    db.site_food_menu.Remove(site_food);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Yemek Silindi.", Message_Type.Success);
                    return RedirectToAction("Menu_Detay", new { id = "" + menu_id });
                }
                else
                {
                    MLog.Error("Yemek Silinemedi.", "Belirtilen Yemek Bulunamadı.");
                    Session["message"] = new MessageModel("HATA", "Belirtilen Yemek Bulunamadı.", Message_Type.Error);

                    return RedirectToAction("Menu_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Yemek Silinemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Menu_Listele");
            }
        }

        #endregion


        #region GALLERY

        public ActionResult Galeri_Listele()
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<gallery_categories> g_categories = db.gallery_categories.ToList();
            ViewData["g_categories"] = g_categories;

            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="GALERİ LİSTELE",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }


        public ActionResult Galeri_Kategori_Detay(string id)
        {
            int titleCategoryId = Convert.ToInt32(id);

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            gallery_categories gallery_category = db.gallery_categories.FirstOrDefault(w => w.id == titleCategoryId);
            ViewData["gallery_category"] = gallery_category;

            List<gallery_titles> gallery_titles = db.gallery_titles.Where(w => w.category_id == titleCategoryId).ToList();
            ViewData["gallery_titles"] = gallery_titles;

            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="GALERİ LİSTELE",url="/Admin_Site/Galeri_Listele"},
                new NavigationModel(){name="GALERİ KATEGORİ DETAY",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }


        // gallery category detail
        public ActionResult Galeri_Detay(string id)
        {
            int albume_id = Convert.ToInt32(id);

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            gallery_titles albume = db.gallery_titles.Where(w => w.id == albume_id).FirstOrDefault();
            ViewData["albume"] = albume;

            List<gallery_images> images = db.gallery_images.Where(w => w.title_id == albume_id).ToList();
            ViewData["albume_images"] = images;



            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="GALERİ LİSTELE",url="/Admin_Site/Galeri_Listele"},
                new NavigationModel(){name="GALERİ KATEGORİ DETAY",url="/Admin_Site/Galeri_Kategori_Detay/"+albume.category_id},
                new NavigationModel(){name="GALERİ DETAY",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        [HttpPost]
        public ActionResult Transition_GaleriTitleEkle(FormCollection formCollection)
        {
            int gallery_category_id = -1;
            try
            {
                gallery_titles albume = (gallery_titles)MTranslation.BuildObject(formCollection, "gallery_titles");
                gallery_category_id = albume.category_id;

                if (albume.title != "")
                {
                    if (albume.keywords == "")
                        albume.keywords = "akgül yemek";

                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.gallery_titles.Add(albume);
                    db.SaveChanges();

                    // creating directory for gallery title's images
                    gallery_titles new_albume = db.gallery_titles.Where(w => w.title == albume.title && w.keywords == albume.keywords).FirstOrDefault();
                    if (new_albume != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Files/Galleries/" + new_albume.id));
                        Directory.CreateDirectory(path);
                    }

                    Session["message"] = new MessageModel("Bilgi", "Galeri Eklendi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Kategori_Detay", new { id = albume.category_id });
                }
                else
                {
                    MLog.Error("Galeri Eklenemedi.", "Eksik veya Yanlış Bilgiler Girildi.");
                    Session["message"] = new MessageModel("ERROR", "Eksik veya Yanlış Bilgiler Girildi.", Message_Type.Error);

                    return RedirectToAction("Galeri_Kategori_Detay", new { id = albume.category_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Galeri Eklenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir Hata Oluştu.", Message_Type.Error);

                return RedirectToAction("Galeri_Kategori_Detay", new { id = gallery_category_id });
            }
        }

        [HttpPost]
        public ActionResult Transition_GaleriTitleGuncelle(FormCollection formCollection)
        {
            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                gallery_titles albume = (gallery_titles)MTranslation.BuildObject(formCollection, "gallery_titles");
                gallery_titles updated_albume = db.gallery_titles.Where(w => w.id == albume.id).FirstOrDefault();

                if (updated_albume != null)
                {
                    if (albume.keywords == "")
                        albume.keywords = "akgül yemek";

                    updated_albume.keywords = albume.keywords;
                    updated_albume.title = albume.title;

                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Galeri Güncellendi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Detay", new { id = albume.id });
                }
                else
                {
                    MLog.Error("Galeri Güncellenemedi.", "Eksik veya Yanlış Bilgiler Girildi.");
                    Session["message"] = new MessageModel("ERROR", "Eksik veya Yanlış Bilgiler Girildi.", Message_Type.Error);

                    return RedirectToAction("Galeri_Detay", new { id = albume.id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Galeri Güncellenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir Hata Oluştu.", Message_Type.Error);

                return RedirectToAction("Galeri_Listele");
            }
        }

        public ActionResult Transition_GaleriTitleSil(string id)
        {
            int gallery_category_id = -1;
            try
            {
                int albume_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                gallery_titles albume = db.gallery_titles.Where(w => w.id == albume_id).FirstOrDefault();
                if (albume != null)
                {
                    gallery_category_id = albume.category_id;

                    // delete albume's images
                    List<gallery_images> images = db.gallery_images.Where(w => w.title_id == albume_id).ToList();
                    if (images.Count > 0)
                        foreach (gallery_images img in images)
                        {
                            db.gallery_images.Remove(img);
                        }
                    db.SaveChanges();

                    // deleting title directory
                    string path = Path.Combine(Server.MapPath("~/Files/Galleries/" + albume_id));
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);

                    // delete albume
                    db.gallery_titles.Remove(albume);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Galeri Silindi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Kategori_Detay", new { id = gallery_category_id });
                }
                else
                {
                    MLog.Error("Galeri Silinemedi.", "Belirtilen Galeri Bulunamadı.");
                    Session["message"] = new MessageModel("HATA", "Belirtilen Galeri Bulunamadı.", Message_Type.Error);

                    return RedirectToAction("Galeri_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Galeri Silinemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                if (gallery_category_id != -1)
                    return RedirectToAction("Galeri_Kategori_Detay", new { id = gallery_category_id });
                else
                    return RedirectToAction("Galeri_Listele");
            }
        }



        [HttpPost]
        public ActionResult Transition_GaleriKategoriEkle(FormCollection formCollection)
        {
            try
            {
                gallery_categories albume_category = (gallery_categories)MTranslation.BuildObject(formCollection, "gallery_categories");

                if (albume_category.name != "")
                {
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.gallery_categories.Add(albume_category);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Galeri Kategorisi Eklendi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Listele");
                }
                else
                {
                    MLog.Error("Galeri Kategorisi Eklenemedi.", "Eksik veya Yanlış Bilgiler Girildi.");
                    Session["message"] = new MessageModel("ERROR", "Eksik veya Yanlış Bilgiler Girildi.", Message_Type.Error);

                    return RedirectToAction("Galeri_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Galeri Kategorisi Eklenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir Hata Oluştu.", Message_Type.Error);
                return RedirectToAction("Galeri_Listele");
            }
        }
        
        [HttpPost]
        public ActionResult Transition_GaleriKategoriGuncelle(FormCollection formCollection)
        {
            try
            {
                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                gallery_categories albume_category = (gallery_categories)MTranslation.BuildObject(formCollection, "gallery_categories");
                gallery_categories updated_albume_category = db.gallery_categories.Where(w => w.id == albume_category.id).FirstOrDefault();

                if (updated_albume_category != null)
                {
                    updated_albume_category.name = albume_category.name;
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Galeri Kategorisi Güncellendi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Kategori_Detay", new { id = albume_category.id });
                }
                else
                {
                    MLog.Error("Galeri Kategorisi Güncellenemedi.", "Eksik veya Yanlış Bilgiler Girildi.");
                    Session["message"] = new MessageModel("ERROR", "Eksik veya Yanlış Bilgiler Girildi.", Message_Type.Error);

                    return RedirectToAction("Galeri_Kategori_Detay", new { id = albume_category.id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Galeri Kategorisi Güncellenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir Hata Oluştu.", Message_Type.Error);

                return RedirectToAction("Galeri_Listele");
            }
        }

        public ActionResult Transition_GaleriKategoriSil(string id)
        {
            try
            {
                int albume_category_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                gallery_categories albume = db.gallery_categories.Where(w => w.id == albume_category_id).FirstOrDefault();
                if (albume != null)
                {
                    // delete albume category
                    db.gallery_categories.Remove(albume);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Galeri Kategorisi Silindi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Listele");
                }
                else
                {
                    MLog.Error("Galeri Kategorisi Silinemedi.", "Belirtilen Galeri Kategorisi Bulunamadı.");
                    Session["message"] = new MessageModel("HATA", "Belirtilen Galeri Kategorisi Bulunamadı.", Message_Type.Error);

                    return RedirectToAction("Galeri_Listele");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Galeri Kategorisi Silinemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Galeri_Listele");
            }
        }



        // gallery element operations

        [HttpPost]
        public ActionResult Transition_GaleriResimEkle(FormCollection formCollection)
        {
            int random_number = new Random().Next(0, 1024);
            int albume_id = 0;

            try
            {
                ext_gallery_images g_image = (ext_gallery_images)MTranslation.BuildObject(formCollection, "ext_gallery_images");
                albume_id = g_image.title_id;

                // adding picture to Disk
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0 && g_image.title_id > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Files/Galleries/" + albume_id), "" + random_number + filename);
                    file.SaveAs(path);

                    // food image_name is uploaded picture name
                    g_image.file_name = "" + random_number + filename;


                    gallery_images save_gimage = new gallery_images()
                    {
                        file_name = g_image.file_name,
                        title_id = g_image.title_id
                    };

                    // adding food to Database
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.gallery_images.Add(save_gimage);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Fotoğraf Galeriye Ekendi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Detay", new { id = "" + albume_id });

                }
                else if (g_image.videoUrl!="")
                {
                    string videoCode = g_image.videoUrl.Split('=')[1];

                    gallery_images save_gimage = new gallery_images()
                    {
                        file_name = videoCode,
                        title_id = g_image.title_id
                    };

                    // adding food to Database
                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    db.gallery_images.Add(save_gimage);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Video Galeriye Ekendi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Detay", new { id = "" + albume_id });
                }
                else
                {
                    MLog.Error("Fotoğraf/Video Galeriye Ekenemedi.", "Eksik Bilgi Girildi.");
                    Session["message"] = new MessageModel("HATA", "Eksik Bilgi Girildi.", Message_Type.Error);

                    return RedirectToAction("Galeri_Detay", new { id = "" + albume_id });
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Fotoğraf Galeriye Ekenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);

                return RedirectToAction("Galeri_Listele");
            }
        }

        public ActionResult Transition_GaleriResimSil(string id)
        {
            int albume_id = 0;

            try
            {
                int gimage_id = Convert.ToInt32(id);

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                gallery_images gimage = db.gallery_images.Where(w => w.id == gimage_id).FirstOrDefault();
                if (gimage != null)
                {
                    albume_id = gimage.title_id;

                    string content_type = (gimage.file_name.Contains(".")) ? "image" : "video";

                    if (content_type == "image")
                    {
                        //deleting picture from disk
                        string file_path = Path.Combine(Server.MapPath("~/Files/Galleries/" + albume_id), gimage.file_name);
                        if (System.IO.File.Exists(file_path))
                            System.IO.File.Delete(file_path);
                    }
                    else if (content_type == "video")
                    {
                        // not need to anything
                    }

                    // deleting from database
                    db.gallery_images.Remove(gimage);
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "Fotoğraf/Video Silindi.", Message_Type.Success);
                    return RedirectToAction("Galeri_Detay", new { id = "" + albume_id });
                }
                else
                {
                    MLog.Error("Fotoğraf/Video Silinemedi.", "Belirtilen Fotoğraf/Video Bulunamadı.");
                    Session["message"] = new MessageModel("HATA", "Belirtilen Fotoğraf Bulunamadı.", Message_Type.Error);

                    return RedirectToAction("Galeri_Detay");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("Fotoğraf Silinemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir hata oluştu.", Message_Type.Error);
                return RedirectToAction("Galeri_Listele");
            }
        }


        #endregion


        #region CONTACT

        public ActionResult Iletisim(string id)
        {

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            Iletisim_Model iletisim_model = new Iletisim_Model()
            {
                address = db.details.Where(w => w.key_ == "address").FirstOrDefault().value,
                email = db.details.Where(w => w.key_ == "email").FirstOrDefault().value,
                tel1 = db.details.Where(w => w.key_ == "tel1").FirstOrDefault().value,
                tel2 = db.details.Where(w => w.key_ == "tel2").FirstOrDefault().value
            };

            ViewData["iletisim_model"] = iletisim_model;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="İLETİŞİM",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________



            return View();
        }

        public ActionResult Transition_IletisimBilgileriniGuncelle(FormCollection formCollection)
        {
            try
            {
                Iletisim_Model iletisim_model = (Iletisim_Model)MTranslation.BuildObject(formCollection, "Iletisim_Model");

                if (iletisim_model.tel1 != "" && iletisim_model.email != "" && iletisim_model.address != "")
                {

                    akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();
                    details detail_address = db.details.Where(w => w.key_ == "address").FirstOrDefault();
                    details detail_email = db.details.Where(w => w.key_ == "email").FirstOrDefault();
                    details detail_tel1 = db.details.Where(w => w.key_ == "tel1").FirstOrDefault();
                    details detail_tel2 = db.details.Where(w => w.key_ == "tel2").FirstOrDefault();

                    detail_address.value = iletisim_model.address;
                    detail_email.value = iletisim_model.email;
                    detail_tel1.value = iletisim_model.tel1;
                    detail_tel2.value = iletisim_model.tel2;
                    db.SaveChanges();

                    Session["message"] = new MessageModel("Bilgi", "İletişim Bilgileri Güncellendi.", Message_Type.Success);
                    return RedirectToAction("Iletisim");
                }
                else
                {
                    MLog.Error("İletişim Bilgileri Güncellenemedi.", "Eksik veya Yanlış Bilgiler Girildi.");
                    Session["message"] = new MessageModel("ERROR", "Eksik veya Yanlış Bilgiler Girildi.", Message_Type.Error);

                    return RedirectToAction("Iletisim");
                }
            }
            catch (Exception exception)
            {
                MLog.Error("İletişim Bilgileri Güncellenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir Hata Oluştu.", Message_Type.Error);

                return RedirectToAction("Iletisim");
            }
        }


        #endregion


        #region CONTENTS

        public ActionResult Icerik(string id)
        {
            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            if (id == null)
                id = "hakkimizda";

            string content = db.details.Where(w => w.key_ == id).FirstOrDefault().value;

            ViewData["id"] = id;
            ViewData["content"] = content;


            //Navigation______________________________________________________
            List<NavigationModel> navList = new List<NavigationModel>() 
            { 
                new NavigationModel(){name="İÇERİK",url=""}
            };
            ViewData["navigation"] = navList;
            //________________________________________________________________


            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Transition_IcerikGuncelle(FormCollection formCollection)
        {
            try
            {
                details contentDetail = (details)MTranslation.BuildObject(formCollection, "details");

                akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

                details willUpdateContentDetail = db.details.Where(w => w.key_ == contentDetail.key_).FirstOrDefault();
                willUpdateContentDetail.value = contentDetail.value;
                db.SaveChanges();

                Session["message"] = new MessageModel("Bilgi", "İçerik Güncellendi.", Message_Type.Success);
                return RedirectToAction("Icerik", new { id = contentDetail.key_ });
            }
            catch (Exception exception)
            {
                MLog.Error("Site Iceriği Güncellenemedi.", exception.Message + Environment.NewLine + exception.StackTrace);
                Session["message"] = new MessageModel("HATA", "Bir Hata Oluştu.", Message_Type.Error);

                return RedirectToAction("Icerik");
            }
        }


        #endregion
    }
}
