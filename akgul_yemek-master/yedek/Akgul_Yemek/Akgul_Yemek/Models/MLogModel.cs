using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Akgul_Yemek.Models
{
    public class MLogModel
    {
        public string datetime { get; set; }
        public string type { get; set; }
        public string caption { get; set; }
        public string detail { get; set; }

        public string log_style { get; set; }

        public static MLogModel Parse(string line)
        {
            string[] pieces = line.Split('#');

            MLogModel log = new MLogModel()
            {
                datetime = pieces[0],
                type = pieces[1],
                caption = pieces[2],
                detail = pieces[3]
            };
            
            switch (log.type.Trim())
            {
                case "ERROR":
                    log.log_style = "danger";
                    break;
                case "FATAL":
                    log.log_style = "danger";
                    break;
                case "Info":
                    log.log_style = "info";
                    break;
                case "Success":
                    log.log_style = "success";
                    break;
                case "Warning":
                    log.log_style = "warning";
                    break;
                case "Debug":
                    log.log_style = "active";
                    break;
                default:
                    log.log_style = "active";
                    break;
            }

            return log;
        }
    }
}
