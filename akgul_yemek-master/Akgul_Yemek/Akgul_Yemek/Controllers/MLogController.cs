using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Akgul_Yemek.Code;
using Akgul_Yemek.Models;

namespace Akgul_Yemek.Controllers
{
    public class MLogController : Controller
    {
        //
        // GET: /MLog/

        public ActionResult Index(string id)
        {
            string logsPath = MLog.GetLogDirectory();

            List<string> logFiles = null;
            if (Directory.Exists(logsPath))
            {
                logFiles = Directory.GetFiles(logsPath).ToList();
                for (int i = 0; i < logFiles.Count; i++)
                {
                    logFiles[i] = Path.GetFileName(logFiles[i]);
                }
            }


            ViewData["logFiles"] = logFiles;

            List<MLogModel> logs = null;

            // read logFile and send to UI
            if (!string.IsNullOrEmpty(id))
            {
                logs = new List<MLogModel>();

                string logFilePath = Path.Combine(logsPath, id);
                using (StreamReader reader = new StreamReader(logFilePath))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        MLogModel log = MLogModel.Parse(line);
                        logs.Add(log);

                        line = reader.ReadLine();
                    }

                    reader.Close();
                }

                ViewData["logs"] = logs;
                ViewData["id"] = id;
            }

            return View();
        }


        public ActionResult Detail(string id)
        {
            string filename = id.Split('-')[0];
            int lineNumber = Convert.ToInt32(id.Split('-')[1]);

            // preventing from nullReferanceException
            MLogModel log = new MLogModel();

            string logsPath = MLog.GetLogDirectory();
            string logFilePath = Path.Combine(logsPath, filename);
            using (StreamReader reader = new StreamReader(logFilePath))
            {
                string line = reader.ReadLine();
                for (int i = 0; line != null; i++)
                {
                    if (lineNumber == i)
                    {
                        if (line.IndexOf('#') != -1)
                        {
                            log = MLogModel.Parse(line);
                        }
                        break;
                    }
                    
                    line = reader.ReadLine();
                }

                reader.Close();
            }

            ViewData["log"] = log;
            ViewData["logFile"] = filename;

            return View();
        }


        public ActionResult Transition_DeleteLogFile(string id)
        {
            string logFileName = id;

            string logsPath = MLog.GetLogDirectory();
            string logFilePath = Path.Combine(logsPath, logFileName);
            if (System.IO.File.Exists(logFilePath))
                System.IO.File.Delete(logFilePath);

            return RedirectToAction("Index");
        }

        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transition_SetServerMappathManual(FormCollection formCollection)
        {
            string serverMappath = formCollection["server_mappath"];
            MLog.SetLogPath(serverMappath);

            return RedirectToAction("Index");
        }
    }
}
