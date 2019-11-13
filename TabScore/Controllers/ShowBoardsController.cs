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
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Session["ContractLevel"] = "";
            Session["ContractSuit"] = "";
            Session["ContractX"] = "NONE";
            Session["NSEW"] = "";
            Session["TricksTakenNumber"] = -1;
            Session["LeadCard"] = "";
            
            List<Result> resList = new List<Result>();
            int iLowBoard = Convert.ToInt32(Session["LowBoard"]);
            int iHighBoard = Convert.ToInt32(Session["HighBoard"]);
            int numBoards = iHighBoard - iLowBoard + 1;

            int resCount = 0;
            for (int i = iLowBoard; i <= iHighBoard; i++)
            {
                Result res = new Result
                {
                    SectionID = Convert.ToInt32(Session["SectionID"]),
                    Table = Convert.ToInt32(Session["Table"]),
                    Round = Convert.ToInt32(Session["CurrentRound"]),
                    Board = i,
                    ContractLevel = null
                };
                if (!res.ReadFromDB(DBConnectionString)) return RedirectToAction("Index", "ErrorScreen");
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
            if (Session["IndividualEvent"].ToString() == "YES")
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["CurrentRound"]} - {Session["PairNS"]}+{Session["South"]} v {Session["PairEW"]}+{Session["West"]}";
            }
            else
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["CurrentRound"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
            }
            ViewData["BackButton"] = "FALSE";

            return View(resList);
        }

        public ActionResult OKButtonClick(string round)  // Gets and passes round as a parameter to avoid double bounce
        {
            return RedirectToAction("Index", "ShowRankingList", new { round, finalRound = "No" });
        }
    }
}