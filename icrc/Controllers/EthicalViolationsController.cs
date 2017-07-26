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
    public class EthicalviolationsController : Controller
    {

        public readonly ICertifiedPersonService CertifiedPersonService;
        public readonly ICertificationService CertificationService;
        public readonly ICertificateService CertificateService;
        public readonly IBoardService BoardService;
        public readonly IStudentEthicalViolationService studentethicalviolationservice;
        public readonly IEthicalViolationService ethicalviolationservice;
        string returnUrl = "";
        string submitUrl = "";
        public EthicalviolationsController(ICertifiedPersonService CertifiedPersonService, ICertificateService CertificateService, ICertificationService CertificationService, IBoardService BoardService, IStudentEthicalViolationService studentethicalviolationservice, IEthicalViolationService ethicalviolationservice)
        {
            this.CertifiedPersonService = CertifiedPersonService;
            this.CertificationService = CertificationService;
            this.CertificateService = CertificateService;
            this.BoardService = BoardService;
            this.studentethicalviolationservice = studentethicalviolationservice;
            this.ethicalviolationservice = ethicalviolationservice;
            ViewBag.CertificatesTexts = new SelectList(CertificateService.GetCertificates(), "Name", "Name");
            ViewBag.BoardsTexts = new SelectList(BoardService.GetBoards(), "Acronym", "Acronym");

        }


        // GET: Ethicalviolations
        public ActionResult Index()
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("/Account/login");
            }
            var data = GetViolations();
            return View(data);
        }
        public void ExportToExcel(string board = "", string person = "", string violation = "")
        {
            var data = GetViolations(board, person, violation).AsEnumerable();
            ShrdMaster.Instance.ExportListFromTsv(data, "StudentEthicalViolations");
        }
        public List<StudentViolationForIndex> GetViolations(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "")
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            List<StudentViolationForIndex> ethicalvoiliation;
            if (ShrdMaster.Instance.IsAdmin(user.Username))
            {
                ethicalvoiliation = studentethicalviolationservice.GetViolationsForIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber, violation).ToList();
            }
            else
            {
                ethicalvoiliation = studentethicalviolationservice.GetEthicalviolationsByBoardID(user.BoardID).ToList();
            }
            return ethicalvoiliation;
        }
        public ActionResult GetData(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "")
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("/Account/login");
            }
            var data = GetViolations(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber, violation);
            return PartialView("_violations", data.OrderByDescending(a => a.ID));
        }
        // GET: Ethicalviolations/Details/5
        public ActionResult Details(int? id)
        {
            SetReturnUrl();
            if (id == null)
            {
                return HttpNotFound();
            }
            var violation = studentethicalviolationservice.GetEthicalviolationByID(id.Value);
            return View(violation);
        }

        // GET: Ethicalviolations/Create
        public ActionResult Create()
        {
            Session.Remove("ReturnURL");
            SetReturnUrl();

            int DD = 0;
            Studentviolations model = new Studentviolations();
            var dd = Session["pID"];
            if (dd != null)
                int.TryParse(Session["pID"].ToString(), out DD);
            var dd1 = CertifiedPersonService.GetCertifiedPersonByID(Convert.ToInt32(DD));
            if (dd1 != null)
            {
             
                model.PersonID = dd1.Id;
                model.PersonName = dd1.FirstName;
                if (dd1.FirstName == "")
                {
                    model.PersonName = dd1.LastName;
                }
                model.Board = dd1.CurrentBoardID;
                ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                //ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
                ViewBag.Ethicalviolations = new SelectList(ethicalviolationservice.GetEthicalviolations(), "ID", "Name");
                return View(model);

            }
            else
            {

                ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                //ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
                ViewBag.Ethicalviolations = new SelectList(ethicalviolationservice.GetEthicalviolations(), "ID", "Name");
                return View(model);
            }
        }



        // POST: Ethicalviolations/Create
        [HttpPost]
        public ActionResult Create(Studentviolations model)
        { 
           
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = SessionContext<int>.Instance.GetSession("UserID");
                studentethicalviolationservice.CreateEthicalviolation(model);
                studentethicalviolationservice.Save();
                return Redirect(Session["ReturnURL"].ToString());
            }
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            //  ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
            ViewBag.Ethicalviolations = new SelectList(ethicalviolationservice.GetEthicalviolations(), "ID", "Name");
            return View(model);
        }

        // GET: Ethicalviolations/Edit/5
        public ActionResult Edit(int? id)
        {
            Session.Remove("ReturnURL");
            SetReturnUrl();
            if (id == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = studentethicalviolationservice.GetEthicalviolationByID(id.Value);
            if (data == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            // ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");

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
            ViewBag.Ethicalviolations = new SelectList(ethicalviolationservice.GetEthicalviolations(), "ID", "Name");
            return View(data);
        }

        // POST: Ethicalviolations/Edit/5
        [HttpPost]
        public ActionResult Edit(Studentviolations model)
        {
          

            if (ModelState.IsValid)
            {
                model.ModifiedAt = DateTime.Now;
                model.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                studentethicalviolationservice.UpdateEthicalviolation(model);
                studentethicalviolationservice.Save();

                return Redirect(Session["ReturnURL"].ToString());
            }

            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
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
            //  ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
            ViewBag.Ethicalviolations = new SelectList(ethicalviolationservice.GetEthicalviolations(), "ID", "Name");
            return View(model);
        }

        // GET: Ethicalviolations/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Ethicalviolations/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                studentethicalviolationservice.Delete(id);
                studentethicalviolationservice.Save();
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
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/Ethicalviolations/Index");
            ViewBag.ReturnURL = returnUrl;
            Session["ReturnURL"] = returnUrl;
            ////to go to previous page
            //if (Request.QueryString["returnUrl"] != null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"];
            //    var arr = returnUrl.Split('/');
            //}

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = ;
            //    ViewBag.ReturnURL = returnUrl;

            //}
            //else
            //{
            //    ViewBag.ReturnURL = returnUrl;
            //}
            // return returnUrl;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
