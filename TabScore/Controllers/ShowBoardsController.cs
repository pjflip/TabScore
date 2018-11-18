using TabScore.Models;
using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index()
        {
            List<ResultClass> resList = new List<ResultClass>();
            int iLowBoard = Convert.ToInt32(Session["LowBoard"]);
            int iHighBoard = Convert.ToInt32(Session["HighBoard"]);
            int numBoards = iHighBoard - iLowBoard + 1;

            int resCount = 0;
            for (int i = iLowBoard; i <= iHighBoard; i++)
            {
                ResultClass res = new ResultClass
                {
                    SectionID = Session["SectionID"].ToString(),
                    Table = Session["Table"].ToString(),
                    Round = Session["Round"].ToString(),
                };
                res.Board = i.ToString();
                res.ContractLevel = null;
                res.GetDBResult(Session["DBConnectionString"].ToString());
                resList.Add(res);
                if (res.ContractLevel != null) resCount++;
            }
            if (resCount == numBoards)
            {
                ViewData["GotAllResults"] = "TRUE";
            }
            else
            {
                ViewData["GotAllResults"] = "FALSE";
            }
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
            ViewData["CancelButton"] = "FALSE";

            return View(resList);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList", new { finalRound = "No" });
        }
    }
}