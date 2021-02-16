using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akgul_Yemek.Models
{
    public class StatisticModel
    {
        public string year { get; set; }

        public List<organization_information> today_org_infos { get; set; }
        public List<organization_information> future_org_infos { get; set; }
        public List<organization_information> nonAccept_org_infos { get; set; }
        public List<organization_information> finished_org_infos { get; set; }
    }
}