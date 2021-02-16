using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akgul_Yemek.Models
{
    public class Extended_Classes
    {
    }

    public class ext_CategoryAndFood
    {
        public food_category f_category { get; set; }
        public List<food> foods { get; set; }

    }

    public class ext_organization_information
    {
        public organization_information org_information { get; set; }

        public ext_organization_information()
        {
        }

        public ext_organization_information(organization_information org_inf)
        {
            org_information = org_inf;
        }


        public string Get_String_ExplainText(string p_user_type, string emailOrSms = "email",bool isForEmail = false)
        {
            string _return = "";

            List<string> lines = null;

            if (emailOrSms == "email")
            {
                lines = GetExplainText(p_user_type, isForEmail);
            }
            else if (emailOrSms == "sms")
            {
                lines = GetExplainTextForSMS();
            }
            foreach (string str_text in lines)
            {
                if (str_text == "line")
                {
                    _return += Environment.NewLine;
                }
                else
                {
                    _return += str_text;
                }
            }

            return _return;
        }

        public List<string> GetExplainText(string p_user_type,bool isForEmail=false)
        {
            List<string> _return = new List<string>();

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_service> base_services = db.organization_service.ToList();
            List<organization_and_food> foods = db.organization_and_food.Where(w => w.organization_information_id == org_information.id).ToList();
            List<organization_and_service> services = db.organization_and_service.Where(w => w.organization_information_id == org_information.id).ToList();

            float calculated_price = 0;

            _return.Add("Organizasyon : " + org_information.organization_name);
            _return.Add("<br/>");
            _return.Add("Org. Sahibi &emsp;: " + org_information.name_surname);
            _return.Add("<br/>");
            _return.Add("Kişi Sayısı&emsp;&emsp;: " + org_information.people_count + "");
            _return.Add("<br/>");

            if (isForEmail)
                _return.Add("<br/>");

            _return.Add("<div style='overflow-x:auto;'>");


            if (isForEmail)
                _return.Add("<table border='1'><thead><tr><th colspan='4'>YEMEKLER</th</tr></thead><tbody>");
            else
                _return.Add("<table class='table table-bordered'><thead><tr><th colspan='4'>YEMEKLER</th</tr></thead><tbody>");

            foreach (organization_and_food org_food in foods)
            {
                calculated_price += org_food.total_price;

                _return.Add("<tr>");

                string str_food;
                if (p_user_type == "0")
                    str_food = "<td>" + org_food.food.name + "</td><td>" + org_food.quantity + " " + org_food.food.food_units.name + "</td><td>" + org_food.people_count + " Kişi </td><td>" + org_food.total_price + " TL </td>";
                else
                    str_food = "<td>" + org_food.food.name + "</td><td>" + org_food.quantity + " " + org_food.food.food_units.name + "</td><td>" + org_food.people_count + " Kişi </td>";

                _return.Add(str_food);


                _return.Add("</tr>");

                _return.Add("line");
            }

            _return.Add("</tbody></table>");
            _return.Add("</div>");

            if(isForEmail)
                _return.Add("<font size='2'>");
            else
                _return.Add("<font size='4'>");

            _return.Add("<br/>");

            foreach (organization_service org_base_serv in base_services)
            {
                organization_and_service org_service = services.Where(w => w.organization_service_id == org_base_serv.id).FirstOrDefault();

                if (org_service == null)
                {
                    continue;
                }


                calculated_price += org_service.price;

                _return.Add(org_service.organization_service.name + ":");
                _return.Add("line");

                string str_service;
                if (p_user_type == "0")
                {
                    str_service = org_service.information + " ( " + org_service.price + " TL )";
                    if (org_service.organization_service.name == "Personel")
                        _return.Add("Aşçı(" + org_information.count_cook + "), Şef(" + org_information.count_chef + "), Garson(" + org_information.count_waiter + "), Diğer(" + org_information.count_other + ")");

                    _return.Add(str_service);
                    _return.Add("line");
                    _return.Add("line");
                }

                _return.Add("line");


            }

            if (isForEmail)
                _return.Add("<br/>");

            if (p_user_type == "0")
            {
                _return.Add("<br/><b>TOPLAM FİYAT: " + calculated_price + " TL</b>");
                _return.Add("line");
            }

            _return.Add("</font>");
            _return.Add("line");

            return _return;
        }



        public List<string> GetExplainTextForSMS()
        {
            List<string> _return = new List<string>();

            akgul_yemek_dbEntities db = new akgul_yemek_dbEntities();

            List<organization_service> base_services = db.organization_service.ToList();
            List<organization_and_food> foods = db.organization_and_food.Where(w => w.organization_information_id == org_information.id).ToList();
            List<organization_and_service> services = db.organization_and_service.Where(w => w.organization_information_id == org_information.id).ToList();


            float calculated_price = 0;

            _return.Add("Sayın ");
            _return.Add(org_information.name_surname);
            _return.Add(", ");
            _return.Add(org_information.organization_type.name);
            _return.Add(", ");
            _return.Add("Tarih:" + org_information.date.ToString("dd.MM.yyyy HH:mm"));
            _return.Add(", ");
            _return.Add("Kisi Sayisi:" + org_information.people_count + "");
            _return.Add(", ");
            _return.Add("Yemekler: ");

            foreach (organization_and_food org_food in foods)
            {
                calculated_price += org_food.total_price;
                _return.Add(org_food.food.name);
                _return.Add(", ");
            }


            foreach (organization_service org_base_serv in base_services)
            {
                organization_and_service org_service = services.Where(w => w.organization_service_id == org_base_serv.id).FirstOrDefault();

                if (org_service == null)
                {
                    continue;
                }

                calculated_price += org_service.price;
            }


            _return.Add("Fiyat:" + calculated_price + "TL");
            _return.Add(". ");
            _return.Add("Akgul Kardesler Catering olarak hizmet sunmaktan onur duyariz.  ");

            return _return;
        }
    }

    public class ext_gallery_images
    {
        public int title_id { get; set; }
        public string file_name { get; set; }
        public string videoUrl { get; set; }
    }
}