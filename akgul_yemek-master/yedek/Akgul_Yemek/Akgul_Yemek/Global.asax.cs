using Akgul_Yemek.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Akgul_Yemek
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // setting MLog
            MLog.SetLogPath(Server.MapPath(""));
            // MLog.Info("Setted value", "Global.asax: Server Mappath setted");
        }



        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 500;// dakika cinsinden
        }

        // Error sayfası için
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var exception = Server.GetLastError();

                var httpException = exception as HttpException;

                // Process 404 HTTP errors
                if (httpException != null && httpException.GetHttpCode() == 404)
                {
                    CreateError("Not Found");

                    //Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;
                }

                // Process 500 HTTP errors
                if (httpException != null && httpException.GetHttpCode() == 500)
                {
                    CreateError("Server Error");

                    //Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;
                }

                if (!Response.IsRequestBeingRedirected)
                    Response.Redirect("/Site/Error");
            }
            catch (Exception exception)
            {
                
            }
        }


        public void CreateError(string errCaption)
        {
            Exception excp = Server.GetLastError();
            //string actor = (Context.Session == null) ? "GUEST" : "USER";
            string message = "URL: " + Request.Url.ToString() + Environment.NewLine + " ERROR: " + excp.Message;
            if (excp.InnerException != null)
                message += Environment.NewLine + " Inner Error: " + excp.InnerException.Message;

            MLog.Error(errCaption, message);

            Server.ClearError();
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            CreateError("Page Error");
        }
    }
}