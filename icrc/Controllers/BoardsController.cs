using ICRC.Model;
using ICRCService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IC_RC.Models;

namespace IC_RC.Controllers
{
    [Authorize]
    public class BoardsController : Controller
    {
        public readonly IBoardService boardService;
        string returnUrl;
        public BoardsController(IBoardService boardService)
        {
            this.boardService = boardService;
        }


        // GET: Boards
        public ActionResult Index()
        {

            var data = GetBoardsList();
            return View(data);
        }


        public void ExportToExcel(string BoardName = "", string BoardAcronym = "")
        {
            var data = GetBoardsList(BoardName, BoardAcronym).AsEnumerable();
            ShrdMaster.Instance.ExportListFromTsv(data, "BoardsData");
        }
        public List<Boards> GetBoardsList(string BoardName = "", string BoardAcronym = "")
        {
            return boardService.GetBoardsForIndex(BoardName,BoardAcronym).ToList();


        }
        public ActionResult GetData(string BoardName="",string BoardAcronym="")
        {

            var data = GetBoardsList(BoardName,BoardAcronym);
            return PartialView("_Boards", data);
        }

        // GET: Boards/Details/5
        public ActionResult Details(int ?id)
        {
            SetReturnUrl();
            if(id==null)
            {
                return HttpNotFound();
            }
            var board = boardService.GetBoardByID(id.Value);
            return View(board);
        }

        // GET: Boards/Create
        public ActionResult Create()
        {
            SetReturnUrl();
            return View();
        }

        // POST: Boards/Create
        [HttpPost]
        public ActionResult Create(Boards model)
        {
            SetReturnUrl();
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = SessionContext<int>.Instance.GetSession("UserID");
                boardService.CreateBoard(model);
                boardService.Save();

                return Redirect(returnUrl);
            }
            return View(model);
        }

        // GET: Boards/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return RedirectToActionPermanent("PageNotFound","Home");
            }
            var data = boardService.GetBoardByID(id.Value);
            if(data==null)
            {
                return HttpNotFound();
            }
            SetReturnUrl();
            return View(data);
        }

        // POST: Boards/Edit/5
        [HttpPost]
        public ActionResult Edit(Boards model)
        {
            SetReturnUrl();
           if(ModelState.IsValid)
            {
                model.ModifiedAt = DateTime.Now;
                model.ModifiedBy = SessionContext<int>.Instance.GetSession("UserID");
                boardService.UpdateBoard(model);
                boardService.Save();

                return Redirect(returnUrl);
            }

            return View(model);

        }


        public ActionResult GetBoards()
        {
            var data = boardService.GetBoards();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Boards/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Boards/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                boardService.Delete(id);
                boardService.Save();
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
            returnUrl = ShrdMaster.Instance.GetReturnUrl("/Boards/Index");
            ViewBag.ReturnURL = returnUrl;

            
        }
    }
}
