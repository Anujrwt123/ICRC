using ICRC.Data.Infrastructure;
using ICRC.Model;
using ICRCService;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_RC.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersController : Controller
    {
        readonly IUsersService userService;
        readonly IUnitOfWork unitOfWork;
        readonly IBoardService boardService;
        string returnUrl;

        public UsersController(IUsersService userService, IUnitOfWork unitOfWork, IBoardService boardService)
        {
            this.userService = userService;
            this.unitOfWork = unitOfWork;
            this.boardService = boardService;
        }

        // GET: Uesrs
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetData()
        {
            var data= userService.GetAllForIndex().ToList();

            return PartialView("_Users", data);
        }

        public ActionResult Create()
        {
            SetReturnUrl();
            ViewBag.Boards = new SelectList(boardService.GetBoards().ToList(), "ID", "Acronym");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Users model)
        {
            SetReturnUrl();
            ViewBag.Boards = new SelectList(boardService.GetBoards().ToList(), "ID", "Acronym");
            if (userService.IsUsernameExists(model.Username))
            {
                ViewBag.Error = "Username already exists.";
                
                return View(model);
            }

            if (ModelState.IsValid)
            {                            
                    model.Active = true;
                    userService.CreateUser(model);
                    userService.Save();
                 var result= ShrdMaster.SendEmail(model.Email, model.Username, model.Password);
                if (result == true)
                {
                    return Redirect(returnUrl);
                }                                                
            }            
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            SetReturnUrl();
            ViewBag.Boards = new SelectList(boardService.GetBoards().ToList(), "ID", "Acronym");
            var data = userService.GetUserByID(ID);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(Users model)
        {
            SetReturnUrl();
            if (ModelState.IsValid)
            {
                userService.UpdateUser(model);
                userService.Save();
                return Redirect(returnUrl);
            }
            ViewBag.Boards = new SelectList(boardService.GetBoards().ToList(), "ID", "Acronym");
            return View(model);
        }



        public ActionResult Delete(int id)
        {
            SetReturnUrl();
            var data=userService.GetUserByID(id);
            userService.Delete(data);
            userService.Save();
            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public void SetReturnUrl()
        {
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/Users/Index");
            ViewBag.ReturnURL = returnUrl;

            ////to go to previous page
            //if (Request.QueryString["returnUrl"] != null)
            //{
            //    returnUrl = Request.QueryString["returnUrl"];
            //}

            //if (string.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = "/Users/Index";
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