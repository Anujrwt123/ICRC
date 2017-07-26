using IC_RC.Models;
using ICRC.Model;
using ICRC.Model.ViewModel;
using ICRCService;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize]
    public class CertifiedPersonsController : Controller
    {
        public readonly ICertifiedPersonService CertifiedPersonService;
        public readonly ICertificationService CertificationService;
        public readonly ICertificateService CertificateService;
        public readonly IReciprocitiesService ReciprocityService;
        public readonly IEthicalViolationService violationservice;
        public readonly IBoardService BoardService;
        public readonly IScoreservice scoreService;
        string returnUrl;
        
        public readonly IStudentEthicalViolationService Stundentethicalviolationservice;


        public CertifiedPersonsController(ICertifiedPersonService CertifiedPersonService,
            ICertificationService CertificationService,
            IReciprocitiesService ReciprocityService,
            IEthicalViolationService violationservice,
            IBoardService BoardService,
            IScoreservice scoreService,
            IStudentEthicalViolationService Stundentethicalviolationservice,
            ICertificateService CertificateService)
        {
            this.CertifiedPersonService = CertifiedPersonService;
            this.CertificationService = CertificationService;
            this.ReciprocityService = ReciprocityService;
            this.violationservice = violationservice;
            this.BoardService = BoardService;
            this.scoreService = scoreService;
            this.Stundentethicalviolationservice = Stundentethicalviolationservice;
            this.CertificateService = CertificateService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = SessionContext<Users>.Instance.GetSession("User");
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("/Account/login");
            }

            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "Acronym", "Acronym");
            ViewBag.Certificates = new SelectList(CertificateService.GetCertificates(), "Name", "Name");

            var data = PersonsData();
            return View(data.OrderByDescending(a => a.ID));
      }

        public void ExportToExcel(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            List<ViewModelForDownloadExcel> excel = new List<ViewModelForDownloadExcel>();
           var data = PersonsData(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber,true).ToList();

            if (data.Count > 0)
            {

                int persId=data[0].ID;
                ViewModelForDownloadExcel excel1 = null;
                foreach (var item in data)
                {
                    if (persId==item.ID)
                    {
                        if (excel1 == null)
                        {
                            excel1 = new ViewModelForDownloadExcel();
                        }
                        excel1.ID = item.ID;

                        if (item.FirstName != null)
                        {
                            
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.FirstName.Trim());                        
                            excel1.FirstName = UTF8Encoding.UTF8.GetString(string_to_send);
                            //excel1.FirstName = Regex.Replace(item.FirstName.Trim(), "[^A-Za-z0-9_. ]+", "");
                        }
                        else
                        {
                            excel1.FirstName = item.FirstName;
                        }
                        if (item.LastName != null) {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.LastName.Trim());
                            excel1.LastName = UTF8Encoding.UTF8.GetString(string_to_send);                    
                        }
                        else
                            excel1.LastName = item.LastName;
                        if (item.address != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.address.Trim());
                            excel1.address = UTF8Encoding.UTF8.GetString(string_to_send);                           
                        }
                        else { excel1.address = item.address; }
                        excel1.MiddleName = item.MiddleName;
                        excel1.Suffix = item.Suffix;
                        if (item.City != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.City.Trim());
                            excel1.City = UTF8Encoding.UTF8.GetString(string_to_send);
                        }
                        else
                            excel1.City = item.City;
                        if (item.State != null) {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.State.Trim());
                            excel1.State = UTF8Encoding.UTF8.GetString(string_to_send);
                            //excel1.State =  Regex.Replace(item.State.Trim(), "[^A-Za-z0-9_. ]+", "");
                        }
                        else {
                            excel1.State = item.State;
                        }
                           excel1.Zip = item.Zip;
                            excel1.Country = item.Country;
                        if (item.Email != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.Email.Trim());
                            excel1.Email = UTF8Encoding.UTF8.GetString(string_to_send);
                            //excel1.State =  Regex.Replace(item.State.Trim(), "[^A-Za-z0-9_. ]+", "");
                        }
                        else
                        {
                            excel1.Email = item.Email;
                        }
                        excel1.Phone = item.Phone;
                            excel1.CurrentBoardID = item.CurrentBoardID;
                            excel1.BoardAcronym = item.BoardAcronym;
                            if (item.Name == "ICAADC")
                            {
                                excel1.CertID_ICAADC =Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICAADC = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICAADC = item.StartDate;
                                excel1.RecertDate_ICAADC = item.RecertDate;
                            }
                            else if (item.Name == "ICADC")
                            {

                                excel1.CertID_ICADC = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICADC = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICADC = item.StartDate;
                                excel1.RecertDate_ICADC = item.RecertDate;
                            }
                            else
                           if (item.Name == "ICCDP")
                            {

                                excel1.CertID_ICCDP = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICCDP = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICCDP = item.StartDate;
                                excel1.RecertDate_ICCDP = item.RecertDate;
                            }
                            else if (item.Name == "ICCDPD")
                            {

                                excel1.CertID_ICCDPD = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICCDPD = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICCDPD = item.StartDate;
                                excel1.RecertDate_ICCDPD = item.RecertDate;
                            }
                            else if (item.Name == "ICCJP")
                            {

                                excel1.CertID_ICCJP = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICCJP = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICCJP = item.StartDate;
                                excel1.RecertDate_ICCJP = item.RecertDate;
                            }
                            else if (item.Name == "ICCS")
                            {

                                excel1.CertID_ICCS = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICCS = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICCS = item.StartDate;
                                excel1.RecertDate_ICCS = item.RecertDate;
                            }
                            else if (item.Name == "ICPS")
                            {

                                excel1.CertID_ICPS = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICPS = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICPS = item.StartDate;
                                excel1.RecertDate_ICPS = item.RecertDate;
                            }
                            else
                            {
                                excel1.CertID_ICPR = Convert.ToInt32(item.CertID);
                                excel1.CertificateNo_ICPR = Convert.ToInt64(item.certificateNo);
                                excel1.StartDate_ICPR = item.StartDate;
                                excel1.RecertDate_ICPR = item.RecertDate;
                            }
                            //excel.Add(excel1);
                    }
                    else
                    {
                        excel.Add(excel1);
                        persId = item.ID;
                        excel1 = null;
                        excel1 = new ViewModelForDownloadExcel();

                        excel1.ID = item.ID;
                        if (item.FirstName != null)
                        {
                            if (item.FirstName == "Humberto")
                            {

                            }
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.FirstName.Trim());
                            excel1.FirstName = UTF8Encoding.UTF8.GetString(string_to_send);
                            //excel1.FirstName = Regex.Replace(item.FirstName.Trim(), "[^A-Za-z0-9_. ]+", "");
                        }
                        else
                        {
                            excel1.FirstName = item.FirstName;
                        }
                        if (item.LastName != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.LastName.Trim());
                            excel1.LastName = UTF8Encoding.UTF8.GetString(string_to_send);
                        }
                        else
                            excel1.LastName = item.LastName;
                        if (item.address != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.address.Trim());
                            excel1.address = UTF8Encoding.UTF8.GetString(string_to_send);
                        }
                        else { excel1.address = item.address; }
                        excel1.MiddleName = item.MiddleName;
                        excel1.Suffix = item.Suffix;
                        if (item.City != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.City.Trim());
                            excel1.City = UTF8Encoding.UTF8.GetString(string_to_send);
                        }
                        else
                            excel1.City = item.City;
                        if (item.State != null)
                        {
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.State.Trim());
                            excel1.State = UTF8Encoding.UTF8.GetString(string_to_send);
                            //excel1.State =  Regex.Replace(item.State.Trim(), "[^A-Za-z0-9_. ]+", "");
                        }
                        else
                        {
                            excel1.State = item.State;
                        }
                        if (item.Email != null)
                        {                          
                            byte[] string_to_send = UTF8Encoding.UTF8.GetBytes(item.Email.Trim());
                            excel1.Email = UTF8Encoding.UTF8.GetString(string_to_send);
                            //excel1.State =  Regex.Replace(item.State.Trim(), "[^A-Za-z0-9_. ]+", "");
                        }
                        else
                        {
                            excel1.Email = item.Email;
                        }
                        excel1.Zip = item.Zip;
                        excel1.Country = item.Country;

                        excel1.Phone = item.Phone;
                        excel1.CurrentBoardID = item.CurrentBoardID;
                        excel1.BoardAcronym = item.BoardAcronym;
                        if (item.Name == "ICAADC")
                        {
                            excel1.CertID_ICAADC = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICAADC = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICAADC = item.StartDate;
                            excel1.RecertDate_ICAADC = item.RecertDate;
                        }
                        else if (item.Name == "ICADC")
                        {

                            excel1.CertID_ICADC = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICADC = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICADC = item.StartDate;
                            excel1.RecertDate_ICADC = item.RecertDate;
                        }
                        else
                       if (item.Name == "ICCDP")
                        {

                            excel1.CertID_ICCDP = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICCDP = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICCDP = item.StartDate;
                            excel1.RecertDate_ICCDP = item.RecertDate;
                        }
                        else if (item.Name == "ICCDPD")
                        {

                            excel1.CertID_ICCDPD = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICCDPD = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICCDPD = item.StartDate;
                            excel1.RecertDate_ICCDPD = item.RecertDate;
                        }
                        else if (item.Name == "ICCJP")
                        {

                            excel1.CertID_ICCJP = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICCJP = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICCJP = item.StartDate;
                            excel1.RecertDate_ICCJP = item.RecertDate;
                        }
                        else if (item.Name == "ICCS")
                        {

                            excel1.CertID_ICCS = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICCS = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICCS = item.StartDate;
                            excel1.RecertDate_ICCS = item.RecertDate;
                        }
                        else if (item.Name == "ICPS")
                        {
                            excel1.CertID_ICPS = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICPS = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICPS = item.StartDate;
                            excel1.RecertDate_ICPS = item.RecertDate;
                        }
                        else
                        {
                            excel1.CertID_ICPR = Convert.ToInt32(item.CertID);
                            excel1.CertificateNo_ICPR = Convert.ToInt64(item.certificateNo);
                            excel1.StartDate_ICPR = item.StartDate;
                            excel1.RecertDate_ICPR = item.RecertDate;
                        }
                    }
                }
            }
            excel.RemoveAll(a => a.FirstName.Length == 1);
            ShrdMaster.Instance.ExportListFromTsv(excel, "CertifiedPersonsData");
        }

        public ActionResult GetPersonView(string returnParam = null)
        {
            int pID = Convert.ToInt32(Session["pID"]);

            if (pID == 0)
            {
                PreCreate();
                ViewBag.Layout = null;
                if (returnParam != null)
                {
                    ViewBag.ReturnURL = returnParam;
                }

                return View("Create");
            }
            else
            {
                var data = CertifiedPersonService.GetCertifiedPersonByID(pID);
                var CB = data.CurrentBoardID;
                var OB = data.OtherBoardID;
                var Suffix = data.Suffix;
                PreCreate1(CB, OB, Suffix);
                ViewBag.Layout = null;
                if (returnParam != null)
                {
                    ViewBag.ReturnURL = returnParam;
                }
                ViewBag.PersonID = data.Id;

                Session["pID"] = 0;
                return View("Create", data);
            }

        }

        public void PreCreate1(int? CB, int? OB, string Suffix)
        {

            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", CB);
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", OB);
            ViewBag.Suffix = new SelectList(ShrdMaster.Instance.GetSuffix(), "ID", "Name", Suffix);
        }

        public IQueryable<CPViewModelForIndex> PersonsData(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "" ,bool excel=false)
        {
            IQueryable<CPViewModelForIndex> data;
            if (ShrdMaster.Instance.IsAdmin(User.Identity.Name))
            {

                data = CertifiedPersonService.GetCertifedPersonsForIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber,excel).AsQueryable();
            }
            else
            {
                var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
                //if(user==null)
                //{
                //    ViewBag.Error = "User is not active.";
                //    return RedirectToAction("Account/login");
                //}
                data = CertifiedPersonService.GetCertifiedPersonsByBoardId(user.BoardID, FirstName, LastName, MiddleName, Acronym, City, State).AsQueryable();
            }
            return data;
        }
        public ActionResult GetData(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string certificate = "", string certificateNumber = "")
         {

            //pageIndex = ShrdMaster.Instance.GetPageIndex();
            //var data1 = CertifiedPersonService.GetCertifedPersonsForIndex(pageIndex).AsQueryable();
            //var data =new CertifiedPersonsGrid(data1,1,true);


            var data = PersonsData(FirstName, LastName, MiddleName, Acronym, "", State, certificate, certificateNumber);
            return PartialView("_CertifiedPersons", data);

        }




        // GET: CertifiedPersons/Details/5
        public ActionResult Details(int? id)
        {

            Session["pID"] = id;
            SetReturnUrl();
            if (id == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }

            var data = CertifiedPersonService.GetCertifiedPersonByID(id.Value);
            if (data == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }

            if (data != null && data.CurrentBoardID > 0)
            {
                ViewBag.CurrentBoard = BoardService.GetBoardByID(data.CurrentBoardID == null ? 0 : data.CurrentBoardID.Value).Acronym;
            }

            return View(data);
        }
        public ActionResult MergePerson(string IDs)
        {

            if (IDs == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            else
            {
                var id = IDs.Split(',');
                CertificationService.Mergeperson(IDs, Convert.ToInt32(id[0]));
                return Json("Merged Successfully", JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult Scores(int? ID)
        {

            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = scoreService.ScoresGetByPersonID(ID.Value);
            return PartialView("_Scores", data);

        }
        //public ActionResult GetPercentage()
        //{
        //    var data = CertificationService.Getpercentage();
        //    return Json(data.BoardCertificateAcronym, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Certificates(int? ID)
        {
            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }

            var data = CertificationService.GetCertificationsByPersonID(ID.Value).ToList();
            return PartialView("_Certifications", data);
        }

        public ActionResult Ethicalviolations(int? ID)
        {
            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = Stundentethicalviolationservice.GetVioltaionsByPersonID(ID.Value).ToList();
            return PartialView("_violations", data);
        }


        public ActionResult Reciprocities(int? ID)
        {
            if (ID == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var data = ReciprocityService.ReciprocityGetByPersonID(ID.Value).ToList();
            return PartialView("_Reciprocities", data);
        }


        // GET: CertifiedPersons/Create
        public ActionResult Create()
        {
            SetReturnUrl();

            PreCreate();
            ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
            return View();
        }

        private void PreCreate()
        {
            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Suffix = new SelectList(ShrdMaster.Instance.GetSuffix(), "ID", "Name");
        }

        // POST: CertifiedPersons/Create
        [HttpPost]
        public ActionResult Create(CertifiedPersons person, bool IsCreate = false)
        {
            SetReturnUrl();
            // TODO: Add insert logic here
            // person.CurrentBoardID
            if (ModelState.IsValid)
            {
                person.CreatedAt = DateTime.Now;
                person.CreatedBy = SessionContext<int>.Instance.GetSession("UserID");
                CertifiedPersonService.CreateCertifiedPerson(person);
                CertifiedPersonService.Save();
                //db.SaveChanges();
                if (IsCreate)
                {
                    return Json(person.Id, JsonRequestBehavior.AllowGet);
                }
                return Redirect(returnUrl);
            }
            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Suffix = new SelectList(ShrdMaster.Instance.GetSuffix(), "ID", "Name");
            return RedirectToAction("index");
        }

        // GET: CertifiedPersons/Edit/5
        public ActionResult Edit(int id)
        {
            SetReturnUrl();
            var data = CertifiedPersonService.GetCertifiedPersonByID(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", data.CurrentBoardID);
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", data.OtherBoardID);
            ViewBag.Suffix = new SelectList(ShrdMaster.Instance.GetSuffix(), "ID", "Name", data.Suffix);
            //Certifications


            return View(data);
        }

        public ActionResult CreateNew(int id)
        {
            SetReturnUrl();
            var data = CertifiedPersonService.GetCertifiedPersonByID(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", data.CurrentBoardID);
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", data.OtherBoardID);
            ViewBag.Suffix = new SelectList(ShrdMaster.Instance.GetSuffix(), "ID", "Name", data.Suffix);
            //Certifications


            return View(data);
        }


        [HttpPost]
        public ActionResult Edit(CertifiedPersons person)
        {
            SetReturnUrl();
            ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", person.CurrentBoardID);
            ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym", person.OtherBoardID);
            ViewBag.Suffix = new SelectList(ShrdMaster.Instance.GetSuffix(), "ID", "Name", person.Suffix);
            // TODO: Add insert logic here
            if (ModelState.IsValid)
            {
                person.ModifiedAt = DateTime.Now;
                person.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                CertifiedPersonService.UpdateCertifiedPerson(person);
                CertifiedPersonService.Save();
                //db.SaveChanges();
                return Redirect(returnUrl);
            }
            return View(person);

            //ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            //ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            //return View(person);
        }

        //[ChildActionOnly]
        //public ActionResult Certifications(int ID)
        //{

        //    var certifications = CertificationService.GetCertificationsByPersonID(ID);

        //    return PartialView("_Certifications",certifications);
        //}

        //[ChildActionOnly]
        //public ActionResult violations(int ID)
        //{
        //    var violations = Stundentethicalviolationservice.GetVoiltaionsByPersonID(ID);

        //    return PartialView("_violations",violations);

        //}

        //[ChildActionOnly]
        //public ActionResult Reciprocity(int ID)
        //{

        //    var data = ReciprocityService.ReciprocityGetByPersonID(ID);           
        //    return PartialView("_Recprocities",data);
        //}





        //// POST: CertifiedPersons/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        ViewBag.CurrentBoardID = new SelectList(BoardService.GetBoards(), "ID", "Name");
        //        ViewBag.OtherBoardID = new SelectList(BoardService.GetBoards(), "ID", "Name");
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: CertifiedPersons/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: CertifiedPersons/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                CertifiedPersonService.Delete(id);
                CertifiedPersonService.Save();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/CertifiedPersons/Index");
            ViewBag.ReturnURL = returnUrl;

        }

        public ActionResult GetPersons(string term)
        {
            var data = PersonsData().Where(x => x.FullName.Contains(term)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
