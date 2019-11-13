using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int newRound)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Session["CurrentRound"] = newRound;
            int maxRounds = DBInfo.MaxRounds(DBConnectionString, Convert.ToInt32(Session["SectionID"]));
            if (maxRounds == -1) return RedirectToAction("Index", "ErrorScreen");
            if (newRound > maxRounds)  // Session complete
            {
                if (Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking) == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList");
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen");
                }
            }

            if (Session["IndividualEvent"].ToString() == "YES")   // An individual event, so Move constructor parameter = true
            {
                if (Convert.ToInt32(Session["PairNS"]) != 0)
                {
                    Move m = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["PairNS"]), true);
                    if (m.Table == -1) return RedirectToAction("Index", "ErrorScreen");
                    ViewData["NorthNewTable"] = m.Table;
                    ViewData["NorthNewDirection"] = m.Direction;
                    if (m.Table == Convert.ToInt32(Session["Table"]) && m.Direction == "North")
                    {
                        ViewData["NorthStayMove"] = "Stay";
                    }
                    else
                    {
                        ViewData["NorthStayMove"] = "Move";
                    }
                }
                if (Session["South"].ToString() != "0")
                {
                    Move m = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["South"]), true);
                    if (m.Table == -1) return RedirectToAction("Index", "ErrorScreen");
                    ViewData["SouthNewTable"] = m.Table;
                    ViewData["SouthNewDirection"] = m.Direction;
                    if (m.Table == Convert.ToInt32(Session["Table"]) && m.Direction == "South")
                    {
                        ViewData["SouthStayMove"] = "Stay";
                    }
                    else
                    {
                        ViewData["SouthStayMove"] = "Move";
                    }
                }
                if (Convert.ToInt32(Session["PairEW"]) != 0)
                {
                    Move m = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["PairEW"]), true);
                    if (m.Table == -1) return RedirectToAction("Index", "ErrorScreen");
                    ViewData["EastNewTable"] = m.Table;
                    ViewData["EastNewDirection"] = m.Direction;
                    if (m.Table == Convert.ToInt32(Session["Table"]) && m.Direction == "East")
                    {
                        ViewData["EastStayMove"] = "Stay";
                    }
                    else
                    {
                        ViewData["EastStayMove"] = "Move";
                    }
                }
                if (Session["West"].ToString() != "0")
                {
                    Move m = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["West"]), true);
                    if (m.Table == -1) return RedirectToAction("Index", "ErrorScreen");
                    ViewData["WestNewTable"] = m.Table;
                    ViewData["WestNewDirection"] = m.Direction;
                    if (m.Table == Convert.ToInt32(Session["Table"]) && m.Direction == "West")
                    {
                        ViewData["WestStayMove"] = "Stay";
                    }
                    else
                    {
                        ViewData["WestStayMove"] = "Move";
                    }
                }
            }
            else  // Not an individual event, so Move constructor parameter = false
            {
                if (Convert.ToInt32(Session["PairNS"]) != 0)
                {
                    Move m = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["PairNS"]), false, "NS");
                    if (m.Table == -1) return RedirectToAction("Index", "ErrorScreen");
                    ViewData["NSNewTable"] = m.Table;
                    ViewData["NSNewDirection"] = m.Direction;
                    if (m.Table == Convert.ToInt32(Session["Table"]) && m.Direction == "NS")
                    {
                        ViewData["NSStayMove"] = "Stay";
                    }
                    else
                    {
                        ViewData["NSStayMove"] = "Move";
                    }
                }

                if (Convert.ToInt32(Session["PairEW"]) != 0)
                {
                    Move m = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["PairNS"]), false, "EW");
                    if (m.Table == -1) return RedirectToAction("Index", "ErrorScreen");
                    ViewData["EWNewTable"] = m.Table;
                    ViewData["EWNewDirection"] = m.Direction;
                    if (m.Table == Convert.ToInt32(Session["Table"]) && m.Direction == "EW")
                    {
                        ViewData["EWStayMove"] = "Stay";
                    }
                    else
                    {
                        ViewData["EWStayMove"] = "Move";
                    }
                }
            }

            string boardsNewTable = BoardMove.GetBoardMoveInfo(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRound, Convert.ToInt32(Session["LowBoard"]));
            if (boardsNewTable == "Error") return RedirectToAction("Index", "ErrorScreen");
            ViewData["BoardsNewTable"] = boardsNewTable;
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]}";
            ViewData["BackButton"] = "FALSE";

            if (Session["IndividualEvent"].ToString() == "YES")
            {
                if (Convert.ToInt32(Session["PairNS"]) == 0 || Convert.ToInt32(Session["PairNS"]) == Convert.ToInt32(Session["MissingPair"]))
                {
                    return View("NSMissingIndividual");
                }
                else if (Convert.ToInt32(Session["PairEW"]) == 0 || Convert.ToInt32(Session["PairEW"]) == Convert.ToInt32(Session["MissingPair"]))
                {
                    return View("EWMissingIndividual");
                }
                else
                {
                    return View("Individual");
                }
            }
            else
            {
                if (Convert.ToInt32(Session["PairNS"]) == 0 || Convert.ToInt32(Session["PairNS"]) == Convert.ToInt32(Session["MissingPair"]))
                {
                    return View("NSMissing");
                }
                else if (Convert.ToInt32(Session["PairEW"]) == 0 || Convert.ToInt32(Session["PairEW"]) == Convert.ToInt32(Session["MissingPair"]))
                {
                    return View("EWMissing");
                }
                else
                {
                    return View("Pair");
                }
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