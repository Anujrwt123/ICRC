using ICRC.Model;
using ICRCService;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize(Roles ="Admin")]
    public class TestScoresController : Controller
    {

        public readonly ITestScoreService testScoreService;
        public readonly IScoreboardService scoreboardService;
        public readonly ITestingCompanyService companyService;
        public readonly IBoardService BoardService;
        string returnUrl;
        public TestScoresController(ITestScoreService testScoreService, IScoreboardService scoreboardService, ITestingCompanyService companyService, IBoardService _BoardService)
        {
            this.BoardService = _BoardService;
            this.testScoreService = testScoreService;
            this.scoreboardService = scoreboardService;
            this.companyService = companyService;
        }

        // GET: TestScores
        public ActionResult Index(int option=0)
        {
            ViewBag.Option = option;
            ViewBag.Boards = new SelectList(BoardService.GetBoards(), "ID", "Acronym");
         
            return View();
        }

        public ActionResult Edit(string firstname ,string lastname,string address1)
        {
            SetReturnUrl();
            ViewBag.Companies = new SelectList(companyService.GetTestingCompanies(), "ID", "Name");
            ViewBag.Boards = new SelectList(scoreboardService.GetScoreboards(), "ID", "Name");
            ViewBag.Status = new SelectList(ShrdMaster.Instance.Getstatus(), "ID", "Name");
            var data = GetScoresData(firstname, lastname, address1);
            return View(data);
        }


        public List<TestScoreViewModel> GetScoresData(string firstname,string lastname,string address1)
        {
            var data = testScoreService.GetDataByFirstAndLastName(firstname, lastname, address1).ToList();

            return data;
        }


        public ActionResult LoadScores(string firstname="", string lastname="", string address1="")
        {
            var data = GetScoresData(firstname, lastname, address1);
            return PartialView("_TestScoresEditView",data);
        }


        public void ExportToExcel(int option = 0, string lastname = "", string firstname = "", string middlename = "", string emailaddress = "", string address1 = "", string address2 = "", string exam = "", string status = "",string boards="")
        {
            var data = testScoreService.GetScoresForIndex(lastname, firstname, middlename, emailaddress, address1, address2, exam, status,boards).AsEnumerable();
            ShrdMaster.Instance.ExportListFromTsv(data, "ScoresData");
        }
        public ActionResult GetData(string lastname="", string firstname="", string middlename="", string emailaddress="", string address1="", string address2="", string exam="", string status="", string boards = "")

        {
           // SetReturnUrl();
            //if(option== 0)
            //{
            //    return PartialView("_TestScoreNormalView");
            //}
            var data = testScoreService.GetScoresForIndex(lastname, firstname, middlename, emailaddress, address1, address2, exam, status,  boards ).ToList();
            

            return PartialView("_TestScore",data);
        }

        public ActionResult NormalView()
        {
            ViewBag.PageNumber = 1;
           // ViewBag.LastName = new SelectList(testScoreService.GetLastNames(1),"LastName","LastName");
            return PartialView("_TestScoreNormalView");
        }


        public ActionResult GetLastNames(int PageNum)
        {
            var data = testScoreService.GetLastNames(PageNum);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchLastNames(string initial)
        {
            var data = testScoreService.GetTestScoreByPerson(initial);
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult downloadExcel()
        {
            string path = Server.MapPath("~/Template/TestScoresTemplate.csv");

            return File(path, "application/vnd.ms-excel", "TestScoresTemplate.csv");
        }



        public ActionResult UploadCSV()
        {
            if(Request.Files.Count>0)
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
                        filename = file.FileName + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-(" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ")";
                    }
                    filename = Path.Combine(Server.MapPath("~/Uploads/TestScores"), filename);
                    file.SaveAs(filename);

                    testScoreService.UploadCSV(filename);

                    return Json("1", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    return Json("2", ex.ToString(), JsonRequestBehavior.AllowGet);
                }
              
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

            
        }

        //public void UploadData(string FilePath)
        //{


        //}

        //public string Name(string filename)
        //{
        //    string path = Server.MapPath("~/Uploads/TestScores");
        //}

        public ActionResult GetFirstNames(string name)
        {
            var data = testScoreService.GetFirstNames(name);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult GetFullData(TestScoreViewModel model)
        //{            
        //     //   var data = testScoreService.GetDataByFirstAndLastName(model);
        //        return Json(data,JsonRequestBehavior.AllowGet);            
        //}


        public ActionResult EditTestScores(int ID)
        {

            var data = testScoreService.GetTestScoresByID(ID);            
            ViewBag.Companies = new SelectList(companyService.GetTestingCompanies(), "ID", "Name");
            ViewBag.Boards = new SelectList(scoreboardService.GetScoreboards(), "ID", "Name");
            ViewBag.Status = new SelectList(ShrdMaster.Instance.Getstatus(), "ID", "Name");
            if(data==null)
            {
                return PartialView("_TestScoreView");
            }
            else
            {
                return PartialView("_TestScoreView", data);
            }
            
        }

        [HttpPost]
        public ActionResult UpdateTestScores(TestScore Model)
        {
            testScoreService.UpdateScores(Model);
            testScoreService.Save();

            return Json("1", JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult UpdateInformation(TestScore Model)
        {
            testScoreService.UpdateTestScore(Model);
            testScoreService.Save();

            return Json("1", JsonRequestBehavior.AllowGet);

        }


        // GET: TestScores/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TestScores/Create
        public ActionResult Create(string returnUrl)
        {
            SetReturnUrl();
            
            ViewBag.Companies = new SelectList(companyService.GetTestingCompanies(), "ID", "Name");
            ViewBag.Boards= new SelectList(scoreboardService.GetScoreboards(), "ID", "Name");
            ViewBag.Status = new SelectList(ShrdMaster.Instance.Getstatus(), "ID", "Name");
            return View();
        }

        // POST: TestScores/Create
        [HttpPost]
        public ActionResult Create(TestScore model)
        {
            SetReturnUrl();
            if (ModelState.IsValid)
            {
              
                testScoreService.CreateTestScore(model);
                testScoreService.Save();

                return Redirect(returnUrl);
            }
            ViewBag.Companies = new SelectList(companyService.GetTestingCompanies(), "ID", "Name");
            ViewBag.Boards = new SelectList(scoreboardService.GetScoreboards(), "ID", "Name");
            ViewBag.Status = new SelectList(ShrdMaster.Instance.Getstatus(), "ID", "Name");
            return View(model);
            //try
            //{
            //    // TODO: Add insert logic here

            //    return RedirectToAction("Index");
            //}
            //catch
            //{
            //    return View();
            //}
        }

        //// GET: TestScores/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    SetReturnUrl();
        //    return View();
        //}

        // POST: TestScores/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            SetReturnUrl();
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TestScores/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: TestScores/Delete/5
       [HttpPost]
        public ActionResult Delete(int id)
        {

            SetReturnUrl();
            // TODO: Add delete logic here
            testScoreService.Delete(id);
            testScoreService.Save();
            return Json("1", JsonRequestBehavior.AllowGet);
            
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/TestScores/Index");
            ViewBag.ReturnURL = returnUrl;
            Session["ReturnUrl"] = returnUrl;
            ////to go to previous page
            //if (Request.QueryString["returnUrl"] != null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"];
            //}

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = "/TestScores/Index";
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
