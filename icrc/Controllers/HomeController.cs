using ICRCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IC_RC.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult PageNotFound()
        {
            return View();
        }

        public readonly ICertifiedPersonService personService;
        
        public HomeController(ICertifiedPersonService personService)
        {
            this.personService = personService;
        }
        public ActionResult Index()
        {
            return RedirectToAction("Index","CertifiedPersons");
        }

        public ActionResult GetData()
        {
           // var data=personService.GetCertifiedPersons();
            return PartialView("_CertifiedPerson");
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