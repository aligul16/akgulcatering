using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akgul_Yemek.Models
{
    public class ReportModel
    {
        public string reportType { get; set; }
        public DateTime startDate { get; set; }
        public DateTime stopDate { get; set; }
    }
}