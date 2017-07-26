using IC_RC.App_Start;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace IC_RC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<ICRC.Data.ICRCEntities>(null);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Run();
        }

        protected void Session_Start()
        {
          
            Session["userID"] = User.Identity.GetUserId();
            Session["username"] = User.Identity.Name;
            
        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            //Response.Flush();
            //Response.Close();
            //Response.End();

            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
        }
      

        protected void Application_AuthenticateRequest()
        {
            //var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            //if (user == null)
            //{
            //    //ViewBag.Error = "User is not active.";
            //    Response.Redirect("/Account/Login");
            //}

            //string url = HttpContext.Current.Request.Url.AbsolutePath;

            //if (url.ToUpper().Contains("EDIT") || url.ToUpper().Contains("DELETE")|| url.ToUpper().Contains("CREATE"))
            //{
            //    if (!ShrdMaster.Instance.IsAdmin(User.Identity.Name))
            //    {
            //        Response.Redirect("/Account/Login");
            //    }
            //}

            //url

        }

        protected void Application_Error()
        {
            //Server.GetLastError();
            //Server.ClearError();
            //Response.Redirect("/Home/Error");
        }
    }

    
}
