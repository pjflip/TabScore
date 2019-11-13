using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ConfirmResultController : Controller
    {
        public ActionResult Index()
        {
            if (Session["ContractLevel"].ToString() == "PASS")
            {
                ViewData["DisplayContract"] = "PASS";
            }
            else
            {
                Result res = new Result
                {
                    Board = Convert.ToInt32(Session["Board"]),
                    ContractLevel = Session["ContractLevel"].ToString(),
                    ContractSuit = Session["ContractSuit"].ToString(),
                    ContractX = Session["ContractX"].ToString(),
                    NSEW = Session["NSEW"].ToString(),
                    TricksTakenNumber = Convert.ToInt32(Session["TricksTakenNumber"]),
                    LeadCard = Session["LeadCard"].ToString()
                };
                ViewData["DisplayContract"] = res.DisplayContract();
                res.CalculateScore();
                if (res.Score > 0)
                {
                    ViewData["Direction"] = "NS";
                    ViewData["Score"] = res.ScoreNS;
                }
                else
                {
                    ViewData["Direction"] = "EW";
                    ViewData["Score"] = res.ScoreEW;
                }
            }

            ViewData["BackButton"] = "TRUE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            Result res = new Result
            {
                SectionID = Convert.ToInt32(Session["SectionID"]),
                Table = Convert.ToInt32(Session["Table"]),
                Round = Convert.ToInt32(Session["CurrentRound"]),
                Board = Convert.ToInt32(Session["Board"]),
                PairNS = Convert.ToInt32(Session["PairNS"]),
                PairEW = Convert.ToInt32(Session["PairEW"]),
                ContractLevel = Session["ContractLevel"].ToString(),
                ContractSuit = Session["ContractSuit"].ToString(),
                ContractX = Session["ContractX"].ToString(),
                NSEW = Session["NSEW"].ToString(),
                TricksTakenNumber = Convert.ToInt32(Session["TricksTakenNumber"]),
                LeadCard = Session["LeadCard"].ToString()
            };
            res.CalculateScore();
            Session["Score"] = res.Score;

            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");
            if (Session["IndividualEvent"].ToString() == "YES")
            {
                res.South = Convert.ToInt32(Session["South"]);
                res.West = Convert.ToInt32(Session["West"]);
                if (res.WriteToDB(DBConnectionString, true) == "Error") return RedirectToAction("Index", "ErrorScreen");
            }
            else
            {
                if (res.WriteToDB(DBConnectionString, false) == "Error") return RedirectToAction("Index", "ErrorScreen");
            }

            return RedirectToAction("Index", "ShowTraveller");
        }

        public ActionResult BackButtonClick()
        {
            if (Session["ContractLevel"].ToString() == "PASS")
            {
                return RedirectToAction("Index", "EnterContract", new { board = Session["Board"].ToString() });
            }
            else
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }
        }
    }
}
