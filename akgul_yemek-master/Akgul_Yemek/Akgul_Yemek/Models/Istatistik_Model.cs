using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akgul_Yemek.Models
{
    public class Admin_Istatistik_Model
    {
        public string username { get; set; }

        public int notApplied_org_count { get; set; }
        public int finished_org_count { get; set; }
        public int future_org_count { get; set; }
        public int completely_finished_org_count { get; set; }

        public int food_count { get; set; }
        public int ready_menu_count { get; set; }


    }

    public class Site_Istatistik_Model
    {
        public string username { get; set; }
        public int slider_count { get; set; }
        public int menu_count { get; set; }
        public int food_count { get; set; }
        public int gallery_count { get; set; }
    }
}