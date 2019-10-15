using TabScore.Models;
using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(string newRound)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            MoveClass m = new MoveClass();
            Session["Round"] = newRound;
            if (Session["PairNS"].ToString() != "0")
            {
                m = Move.GetMoveInfo(DBConnectionString, Session["SectionID"].ToString(), newRound, Session["PairNS"].ToString(), "NS");
                if (m.Table == "Error") return RedirectToAction("Index", "ErrorScreen");
                if (m.Table == "0")
                {
                    // No move possible, so session complete
                    if (Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking) == 2)
                    {
                        return RedirectToAction("Index", "ShowRankingList", new { round = newRound, finalRound = "Yes" });
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
                m = Move.GetMoveInfo(DBConnectionString, Session["SectionID"].ToString(), newRound, Session["PairEW"].ToString(), "EW");
                if (m.Table == "Error") return RedirectToAction("Index", "ErrorScreen");
                if (m.Table == "0")
                {
                    // No move possible, so session complete
                    if (Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking) == 2)
                    {
                        return RedirectToAction("Index", "ShowRankingList", new { round = newRound, finalRound = "Yes" });
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

            string boardsNewTable = Move.GetBoardMoveInfo(DBConnectionString, Session["SectionID"].ToString(), newRound, Session["LowBoard"].ToString());
            if (boardsNewTable == "Error") return RedirectToAction("Index", "ErrorScreen");
            ViewData["BoardsNewTable"] = boardsNewTable;
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
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.NumberEntryEachRound))
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