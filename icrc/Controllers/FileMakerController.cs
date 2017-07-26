using ICRCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize(Roles ="Admin")]
    public class FileMakerController : Controller
    {
        public readonly IFileMakerService FileMakerService;
        string returnUrl;
        public FileMakerController(IFileMakerService FileMakerService)
        {
            this.FileMakerService = FileMakerService;
        }
        // GET: FileMaker
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Certificates()
        {
            var data = FileMakerService.GetFileMakerData().ToList();
           Session["page"] = "Certificates";
            return PartialView("_FileMaker",data);
        }

        public ActionResult Reciprocities()
        {
            var data = FileMakerService.GetReciprocities().ToList();
            Session["page"] = "Reciprocities";
            return PartialView("_FileMakerReciprocities", data);
        }

        // GET: FileMaker/Details/5
        public ActionResult Details(int id)
        {
            SetReturnUrl();
            return View();
        }

        // GET: FileMaker/Create
        public ActionResult Create()
        {
            SetReturnUrl();
            return View();
        }

        // POST: FileMaker/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                SetReturnUrl();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FileMaker/Edit/5
        public ActionResult Edit(int id)
        {
            SetReturnUrl();
            return View();
        }

        // POST: FileMaker/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                SetReturnUrl();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FileMaker/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FileMaker/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/FileMaker/Index");
            ViewBag.ReturnURL = returnUrl;
            ////to go to previous page
            //if (Request.QueryString["returnUrl"] != null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"];
            //    var arr = returnUrl.Split('/');
            //}

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = "/FileMaker/Index";
            //    ViewBag.ReturnURL = returnUrl;

            //}
            //else
            //{
            //    ViewBag.ReturnURL = returnUrl;
            //}
            //// return returnUrl;
        }
    }
}
