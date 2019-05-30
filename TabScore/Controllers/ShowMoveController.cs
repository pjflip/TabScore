using TabScore.Models;
using System;
using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            // Go to the next round
            Session["Round"] = (Convert.ToInt32(Session["Round"]) + 1).ToString();

            MoveClass m = new MoveClass();
            if (Session["PairNS"].ToString() != "0")
            {
                m = Move.GetMoveInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Round"].ToString(), Session["PairNS"].ToString(), "NS");
                if (m.Table == "0")
                {
                    // No move possible, so session complete
                    if (Settings.ShowRanking(DBConnectionString) == 2)
                    {
                        return RedirectToAction("Index", "ShowRankingList", new { finalRound = "Yes" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "EndScreen");
                    }
                }
                ViewData["NSNewTable"] = m.Table;
                ViewData["NSNewDirection"] = m.Direction;
                if (m.Table == Session["Table"].ToString() && m.Direction == "NS")
                {
                    ViewData["StayMoveNS"] = "Stay";
                }
                else
                {
                    ViewData["StayMoveNS"] = "Move";
                }
            }

            if (Session["PairEW"].ToString() != "0")
            {
                m = Move.GetMoveInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Round"].ToString(), Session["PairEW"].ToString(), "EW");
                if (m.Table == "0")
                {
                    // No move possible, so session complete
                    if (Settings.ShowRanking(DBConnectionString) == 2)
                    {
                        return RedirectToAction("Index", "ShowRanking", new { finalRound = "Yes" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "EndScreen");
                    }
                }
                ViewData["EWNewTable"] = m.Table;
                ViewData["EWNewDirection"] = m.Direction;
                if (m.Table == Session["Table"].ToString() && m.Direction == "EW")
                {
                    ViewData["StayMoveEW"] = "Stay";
                }
                else
                {
                    ViewData["StayMoveEW"] = "Move";
                }
            }

            ViewData["BoardsNewTable"] = Move.GetBoardMoveInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Round"].ToString(), Session["LowBoard"].ToString());
            ViewData["LowBoard"] = Session["LowBoard"];
            ViewData["HighBoard"] = Session["HighBoard"];
            
            ViewData["Round"] = Session["Round"].ToString();
            ViewData["PairNS"] = Session["PairNS"];
            ViewData["PairEW"] = Session["PairEW"];
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";
            ViewData["BackButton"] = "FALSE";

            if (Session["PairNS"].ToString() == "0" || Session["PairNS"].ToString() == Session["MissingPair"].ToString())
            {
                return View("NSMissing");
            }
            else if (Session["PairEW"].ToString() == "0" || Session["PairEW"].ToString() == Session["MissingPair"].ToString())
            {
                return View("EWMissing");
            }
            else
            {
                return View();
            }
        }
        
        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            if (Settings.NumberEntryEachRound(DBConnectionString))
            {
                return RedirectToAction("Index", "ShowPlayerNumbers");
            }
            else
            {
                return RedirectToAction("Index", "ShowRoundInfo");
            }
        }
    }
}