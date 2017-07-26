using IC_RC.Models;
using IC_RC.ViewModels;
using ICRC.Model;
using ICRCService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using Newtonsoft.Json;
using ICRC.Model.ViewModel;
using System.Data;
using System.Threading.Tasks;
using System.Net;
using ICRC.Data.Repositories;

namespace IC_RC.Controllers
{
    [Authorize]
    public class CertificationsController : Controller
    {

        public readonly ICertifiedPersonService CertifiedPersonService;
        public readonly ICertificationService CertificationService;
        public readonly ICertificateService CertificateService;
        public readonly IBoardService BoardService;
        public readonly IPaymentTypeService paymenttypeService;
        string returnUrl="";

        public CertificationsController(ICertifiedPersonService CertifiedPersonService, 
            ICertificateService CertificateService, 
            ICertificationService CertificationService, 
            IBoardService BoardService, 
            IPaymentTypeService paymenttypeService)
        {
            this.CertifiedPersonService = CertifiedPersonService;
            this.CertificationService = CertificationService;
            this.CertificateService = CertificateService;
            this.BoardService = BoardService;
            this.paymenttypeService = paymenttypeService;


            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.Certificates = new SelectList(CertificateService.GetCertificates(), "ID", "Name");

            ViewBag.CertificatesTexts = new SelectList(CertificateService.GetCertificates(), "Name", "Name");
            ViewBag.BoardsTexts = new SelectList(BoardService.GetBoards(), "Acronym", "Acronym");

        }

        // GET: Certifications
        public ActionResult Index()
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return RedirectToAction("/Account/login");
            }
            var data = GetCertifications();
            return View(data.OrderByDescending(a=>a.ID));
        }
        public void ExportToExcel(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            var data = GetCertifications(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber).AsQueryable();
            ShrdMaster.Instance.ExportListFromTsv(data, "CertificationsData");
        }
        public bool stop()
        {
         CertificationService.stop();
            return true;
        }
        public List<CertificationViewModelForIndex> GetCertifications(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            // var userID = SessionContext<int>.Instance.GetSession("UserID");
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            List<CertificationViewModelForIndex> certifications;
            if (ShrdMaster.Instance.IsAdmin(user.Username))
            {
                certifications = CertificationService.GetCertificationsForIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber).ToList();
            }
            else
            {
                certifications = CertificationService.GetCertificationsByBoardID(user.BoardID, FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber).ToList();
            }
            return certifications;
        }

        public ActionResult GetData(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            var user = ShrdMaster.Instance.GetUser(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Error = "User is not active.";
                return Redirect("Account/login");
            }

            var data = GetCertifications(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber);
            return PartialView("_Certifications",data);
        }


        public int GenerateNumber()
        {
            Random r = new Random();
            int number= r.Next(100000, 999999);                
            if (CertificationService.CheckNumber(number))
            {
                //number = r.Next(100000, 999999);
                GenerateNumber();
            }            
            return number;
        }


        public JsonResult GenerateCertificateNumber()
        {

            int result = GenerateNumber();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Certifications/Details/5
        public ActionResult Details(int ?id)
        {
            SetReturnUrl();
            if (id==null)
            {
                return HttpNotFound();
            }
            var data = CertificationService.GetCertificationByID(id.Value);
            return View(data);
        }

        // GET: Certifications/Create
        public ActionResult Create()
        {
            Session.Remove("ReturnURL");
            SetReturnUrl();
            int DD = 0;
            var dd = Session["pID"];

            if (dd != null)
            {
                int.TryParse(Session["pID"].ToString(), out DD);
                var dd1 = CertifiedPersonService.GetCertifiedPersonByID(Convert.ToInt32(DD));
                if (dd1 != null)
                {
                    ViewBag.IssueBoard = dd1.CurrentBoardID;
                    ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                    ViewBag.PersonName = dd1.LastName;
                    if (dd1.LastName == "")
                    {
                        ViewBag.PersonName = dd1.FirstName;
                    }
                  
                    ViewBag.PersonID = dd1.Id;
                }
            }

                ViewBag.Fees = new SelectList(ShrdMaster.Instance.GetFees(), "ID", "Name");
                // ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(),"ID","FullName");

                ViewBag.Certificates = new SelectList(CertificateService.GetCertificates(), "ID", "Name");
                ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
                ViewBag.CertificateNumber = GenerateNumber();
                ViewBag.PaymentTypes = paymenttypeService.GetPaymentTypes();
                return View();
            
           
        }

        [HttpPost]
        public ActionResult Create(Certifications model)
        {
          
            //int? dd = model.PersonID; 
            
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = SessionContext<int>.Instance.GetSession("UserID");
                CertificationService.CreateCertification(model);
                CertifiedPersonService.Save();
                return Redirect(Session["ReturnURL"].ToString());
            }
            ViewBag.Fees = new SelectList(ShrdMaster.Instance.GetFees(), "ID", "Name");
            //ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
            ViewBag.Certificates = new SelectList(CertificateService.GetCertificates(), "ID", "Name");
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            ViewBag.CertificateNumber = GenerateNumber();
            ViewBag.PaymentTypes = paymenttypeService.GetPaymentTypes();
            return View(model);
        }




        // GET: Certifications/Edit/5
        public ActionResult Edit(int id)
        {
        
            SetReturnUrl();
            var data = CertificationService.GetCertificationByID(id);

            if(data == null)
            {
                return RedirectToActionPermanent("PageNotFound", "Home");
            }
            var person = CertifiedPersonService.GetCertifiedPersonByID(data.PersonID ?? data.PersonID.Value);
            string name = "";
            if (person != null)
            {
                if(!string.IsNullOrEmpty(person.FirstName))
                {
                    name += person.FirstName+" ";
                }
                if(!string.IsNullOrEmpty(person.MiddleName))
                {
                    name += person.MiddleName+" ";
                }
                if (!string.IsNullOrEmpty(person.LastName))
                {
                    name += person.LastName+ " ";
                }
                

                //name = string.Format($"{person.FirstName != null ? person.FirstName + " ":\"\"}");
                //name = person.FirstName != null ? person.FirstName + " " + person.MiddleName != null ? person.MiddleName + " " + person.LastName != null ? person.LastName:"":"":"";
            }
            ViewBag.Person = name;
            ViewBag.Fees = new SelectList(ShrdMaster.Instance.GetFees(), "ID", "Name",data.CertRequestFee);
            // ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
            ViewBag.Certificates = new SelectList(CertificateService.GetCertificates(), "ID", "Name");
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
          
            ViewBag.PaymentTypes = paymenttypeService.GetPaymentTypes();
            ViewBag.QToPrn = data.AddToPrintQueues;
            return View(data);
        }
        public ActionResult AddQueue(string IDs)
        {
            if (IDs != null && IDs != "")
            {
                var id = IDs.Split(',');
                foreach (var item in id)
                {
                 var model= CertificationService.GetCertificationByID(Convert.ToInt32(item));
                    model.AddToPrintQueues = true;
                    CertificationService.UpdateCertification(model);
                    CertificateService.Save();
                }
            }
            return Json("");
        }
        // POST: Certifications/Edit/5
        [HttpPost]
        public ActionResult Edit(Certifications model)
        {
           
           if(ModelState.IsValid)
            {
                model.ModifiedAt = DateTime.Now;
                model.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                CertificationService.UpdateCertification(model);
                CertificateService.Save();
                returnUrl = Session["ReturnURL"].ToString();
                return Redirect(returnUrl);
            }

            var person = CertifiedPersonService.GetCertifiedPersonByID(model.PersonID ?? model.PersonID.Value);
            string name="";
            if (person!=null)
            {
                if (!string.IsNullOrEmpty(person.FirstName))
                {
                    name += person.FirstName = " ";
                }
                if (!string.IsNullOrEmpty(person.MiddleName))
                {
                    name += person.MiddleName + " ";
                }
                if (!string.IsNullOrEmpty(person.LastName))
                {
                    name += person.LastName + " ";
                }
            }
            ViewBag.Person = name;
            ViewBag.Fees = new SelectList(ShrdMaster.Instance.GetFees(), "ID", "Name",model.CertRequestFee);
            //  ViewBag.Persons = new SelectList(CertifiedPersonService.GetCertifedPersonsForIndex(), "ID", "FullName");
            ViewBag.Certificates = new SelectList(CertificateService.GetCertificates(), "ID", "Name");
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
            
            ViewBag.PaymentTypes = paymenttypeService.GetPaymentTypes();
            return View(model);
                    
                
        }

        //// GET: Certifications/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Certifications/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                CertificationService.Delete(id);
                CertificationService.Save();
                return RedirectToAction("GetData");
            }
            catch
            {
                return View();
            }
        }

        public void abort(Object state)
        {
          
     HttpWebRequest request = state as  HttpWebRequest;
            if (request != null)
            { request.Abort(); }
        }

        public void SetReturnUrl()
        {
            
            //to go to previous page
            
                returnUrl =ShrdMaster.Instance.GetReturnUrl("/Certifications/Index");
                Session["ReturnURL"] = returnUrl;
            ViewBag.ReturnURL = returnUrl;


            // return returnUrl;
        }

        public async Task<ActionResult> UploadCertifications()
         {
            ProgressHub.loop = true;
            ProgressHub.count = 0;
            CertificationsRepository.running = true;
            
            
            try
            {
                LoggerApp.Logger.ExceptionPath = Server.MapPath("~/Errors.txt");
                if (Request.Files.Count > 0)
                {
                  
                    try
                    {
                        HttpFileCollectionBase files = Request.Files;
                        HttpPostedFileBase file = files[0];
                        string filename = "";

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            filename = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            filename = file.FileName;// + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-(" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ")";
                        }
                        filename = Path.Combine(Server.MapPath("~/Uploads/Certifications"), filename);
                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }
                        file.SaveAs(filename);
                        var user = SessionContext<Users>.Instance.GetSession("User");
                        if (user == null)
                        {
                            return Json("-3", JsonRequestBehavior.AllowGet);
                        }
                        CertificationService.UploadCSV(filename, user);
                        // testScoreService.UploadCSV(filename);

                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                    catch(Exception ex)
                    {
                        return Json(ex.Message, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                else
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                LoggerApp.Logger.Instance.LogException(ex);
                return Json(ex.InnerException!=null?ex.InnerException.Message:ex.Message, JsonRequestBehavior.AllowGet);
            }

           // return Json("-1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult ShowAddtoQueueCertificates()
        {   
            var data = CertificationService.QueueForPrint();
            
            return View(data.OrderBy(a=>a.PersonName));
        }
        public ActionResult ShowCertificationQueue()
        {
            var data = CertificationService.QueueForPrint();
            Session["queuedata"] = data.Count();
            return PartialView("_QueueCertificates",data);
        }


        public ActionResult PrintBatchCertificates(string ids)
        {
            List<Certifications> data = new List<Certifications>();
            string path= Server.MapPath("~/PrintedCertifications");
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                FileInfo[] files = dir.GetFiles();
                foreach (var file in files)
                {
                    file.Delete();
                }
                //DirectoryInfo[] di = dir.GetDirectories();
                //foreach (var directory in di)
                //{
                //    directory.Delete();
                //}

                //Directory.Delete(path);
            }

            path = Path.Combine(path,"Certifications");
            List<int> cetificates;
            if (!string.IsNullOrEmpty(ids))
            {            
                cetificates=JsonConvert.DeserializeObject<List<int>>(ids);                
            }
            else
            { 
                data = CertificationService.QueueForPrint().ToList();
                cetificates = data.Select(x => x.ID).ToList();                                
            }
            
            CertificationService.GenerateCertificate(cetificates, path);           
            string zipFolder;
           // string zipFolder= Path.Combine(Server.MapPath("~/ PrintedCertifications"),"ZipCertifications.zip");
            using (ZipFile zp = new ZipFile())
            {
                zipFolder = Server.MapPath("~/PrintedCertifications/Certifications.zip");

                string[] files = Directory.GetFiles(path);
                zp.AddFiles(files,"files");
                zp.Save(zipFolder);
             
            }
           
           
            return File(zipFolder, "application/zip", "PrintBatchCertificates.zip");
         
        }

        [HttpPost]
        public ActionResult ClearQueue(string ids)
        {
            
            try
            {
                CertificationService.ClearQueue(ids);                
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
           
        }

        [AllowAnonymous]
        public ActionResult  PrintCertificate(int ID)
        {
            var data= CertificationService.QueueToPrintByCertificationID(ID);
            return View(data);
        }

        public ActionResult downloadExcel()
        {
            string path=Server.MapPath("~/Template/CeritficationTemplate.Csv");
            
            return File(path,"application/vnd.ms-excel", "CertificationTemplate.Csv");
        }



     
    }
}
