using ISAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISAD.Controllers
{
    [AuthorizedUserAttribute]
    public class HomeController : Controller
    {
        private readonly ISADDBEntities db = new ISADDBEntities();
        public ActionResult Index()
        {
            try
            {

            
            int version = Convert.ToInt32(Request.Cookies.Get("Isad-version")?.Value);
            if (version > 0)
            { 
                int lastVersion = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Version"]);
                if(version!=lastVersion)
                {
                    var newVersion = db.Versions.FirstOrDefault(a => a.Version1 == lastVersion);
                    var cookie = Request.Cookies.Get("Isad-version");
                    cookie.Expires.AddYears(1);
                    cookie.Value = lastVersion.ToString();
                    Response.Cookies.Add(cookie);
                    TempData["msg"] = newVersion.New;
                }
            }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}