using IC_RC.Models;
using ICRC.Model;
using ICRC.Model.ViewModel;
using ICRCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize]
    public class ReciprocitiesController : Controller
    {
        public readonly ICertifiedPersonService CertifiedPersonService;
        public readonly IReciprocitiesService reciprocityService;
        public readonly ICertificateService certificateService;
        public readonly IBoardService BoardService;
        public readonly IPaymentTypeService paymentService;
        string returnUrl = "";
        Users user;

        public ReciprocitiesController(ICertifiedPersonService CertifiedPersonService, IReciprocitiesService reciprocityService, IBoardService BoardService, ICertificateService certificateService, IPaymentTypeService paymentService)
        {
            this.CertifiedPersonService = CertifiedPersonService;
            this.reciprocityService = reciprocityService;
            this.BoardService = BoardService;
            this.certificateService = certificateService;
            this.paymentService = paymentService;
            ViewBag.CertificatesTexts = new SelectList(certificateService.GetCertificates(), "Name", "Name");
            ViewBag.BoardsTexts = new SelectList(BoardService.GetBoards(), "Acronym", "Acronym");

        }


        // GET: Reciprocity
        public ActionResult Index()
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("/Account/login");
            }
            var data = GetReciprocities();
           
            return View(data);
        }
        public void ExportToExcel(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            var data = GetReciprocities(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber).AsEnumerable();
            ShrdMaster.Instance.ExportListFromTsv(data, "Reciprocities");
        }
        public List<ReciprocitiesForIndex> GetReciprocities(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);

            List<ReciprocitiesForIndex> reciprocities;
            if (ShrdMaster.Instance.IsAdmin(user.Username))
            {
                reciprocities = reciprocityService.GetReciprocitiesForIndex(FirstName, LastName, MiddleName,Acronym,City,State,Certificate,CertificateNumber).ToList();
            }
            else
            {
                reciprocities = reciprocityService.GetReciprocitiesByBoardID(user.BoardID, FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber).ToList();
            }

            return reciprocities;
        }

        public ActionResult GetData(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            var reciprocities = GetReciprocities(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber);
            return PartialView("_Reciprocities",reciprocities);
        }


        // GET: Reciprocity/Details/5
        public ActionResult Details(int ?id)
        {
            SetReturnUrl();
            if(id==null)
            {
                return HttpNotFound();
            }
            var data = reciprocityService.GetReciprocitiesByID(id.Value);

            if (data != null && data.OriginatingBoard > 0)
            {
                ViewBag.CurrentBoard = BoardService.GetBoardByID(data.OriginatingBoard == null ? 0 : data.OriginatingBoard.Value).Acronym;
            }
            return View(data);
        }

        // GET: Reciprocity/Create
        public ActionResult Create()
        {
            Session.Remove("ReturnURL");
            SetReturnUrl();

            int DD = 0;
            Reciprocities model = new Reciprocities();
            var dd = Session["pID"];
            if(dd!=null)
            int.TryParse(Session["pID"].ToString(), out DD);
            var dd1 = CertifiedPersonService.GetCertifiedPersonByID(Convert.ToInt32(DD));
            if (dd1 != null)
            {
             
                model.PersonID = dd1.Id;
                model.FirstName = dd1.FirstName;
               
                model.LastName = dd1.LastName;
                model.MiddleName = dd1.MiddleName;
                model.OriginatingBoard = dd1.CurrentBoardID;
                ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
                ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
                return View(model);

            }
            else
            {
                ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
                ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
                // ViewBag.Persons= new SelectList(CertifiedPersonService.GetCertifiedPersons(), "ID", "FullName");
                return View(model);
            }
        }

        // POST: Reciprocity/Create
        [HttpPost]
        public ActionResult Create(Reciprocities model)
        {
           
            if(ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = SessionContext<int>.Instance.GetSession("UserID"); 
                reciprocityService.CreateReciprocity(model);
                reciprocityService.Save();
                return Redirect(Session["ReturnURL"].ToString());
            }

            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
            //ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifiedPersons(), "ID", "FullName");
            return View(model);
        }

        // GET: Reciprocity/Edit/5
        public ActionResult Edit(int ?id)
        {
            Session.Remove("ReturnURL");
            SetReturnUrl();
            if(id==null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = reciprocityService.GetReciprocitiesByID(id.Value);
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
            var person = CertifiedPersonService.GetCertifiedPersonByID(data.PersonID ?? data.PersonID.Value);
            string name = "";
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    name += person.FirstName + " ";
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    name += person.MiddleName + " ";
                }
                if (!string.IsNullOrEmpty(person.LastName))
                {
                    name += person.LastName + " ";
                }

                //name = string.Format($"{person.FirstName != null ? person.FirstName + " ":\"\"}");
                //name = person.FirstName != null ? person.FirstName + " " + person.MiddleName != null ? person.MiddleName + " " + person.LastName != null ? person.LastName:"":"":"";
            }
            ViewBag.Person = name;
          

            return View(data);
        }

        // POST: Reciprocity/Edit/5
        [HttpPost]
        public ActionResult Edit(Reciprocities model)
        {
           
            model.Status = true;
           if(ModelState.IsValid)
            {
                model.ModifiedAt = DateTime.Now;
                model.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                reciprocityService.UpdateReciprocity(model);
                reciprocityService.Save();
                return Redirect(returnUrl);
            }

            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(certificateService.GetCertificates(), "ID", "Name");
            ViewBag.PaymentTypes = paymentService.GetPaymentTypes();
            var person = CertifiedPersonService.GetCertifiedPersonByID(model.PersonID ?? model.PersonID.Value);
            string name = "";
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    name += person.FirstName + " ";
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    name += person.MiddleName + " ";
                }
                if (!string.IsNullOrEmpty(person.LastName))
                {
                    name += person.LastName + " ";
                }

                //name = string.Format($"{person.FirstName != null ? person.FirstName + " ":\"\"}");
                //name = person.FirstName != null ? person.FirstName + " " + person.MiddleName != null ? person.MiddleName + " " + person.LastName != null ? person.LastName:"":"":"";
            }
            ViewBag.Person = name;
            return View(model);
        }

        // GET: Reciprocity/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Reciprocity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                reciprocityService.Delete(id);
                reciprocityService.Save();
                return Json(true, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/Reciprocities/Index");
            ViewBag.ReturnURL = returnUrl;
            Session["ReturnURL"] = returnUrl;
            ////to go to previous page
            //if (Request.QueryString["returnUrl"] != null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"];
            //}

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = "/Reciprocities/Index";
            //    ViewBag.ReturnURL = returnUrl;

            //}
            //else
            //{
            //    ViewBag.ReturnURL = returnUrl;
            //}
            // return returnUrl;
        }
    }
}
