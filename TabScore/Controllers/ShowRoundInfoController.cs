using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Round round;
            if (Session["IndividualEvent"].ToString() == "YES")
            {
                round = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), Convert.ToInt32(Session["CurrentRound"]), true);
                if (round.PairNS == -1) return RedirectToAction("Index", "ErrorScreen");
                Session["PairNS"] = round.PairNS.ToString();
                Session["South"] = round.South.ToString();
                Session["PairEW"] = round.PairEW.ToString();
                Session["West"] = round.West.ToString();
            }
            else
            {
                round = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), Convert.ToInt32(Session["CurrentRound"]), false);
                if (round.PairNS == -1) return RedirectToAction("Index", "ErrorScreen");
                Session["PairNS"] = round.PairNS.ToString();
                Session["PairEW"] = round.PairEW.ToString();
            }
            Session["LowBoard"] = round.LowBoard;
            Session["HighBoard"] = round.HighBoard;

            if (Convert.ToInt32(Session["CurrentRound"]) == 1)
            {
                ViewData["BackButton"] = "FALSE";
            }
            else 
            {
                ViewData["BackButton"] = "TRUE";
            }
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            string tempName;
            if (round.PairNS == 0 || round.PairNS == Convert.ToInt32(Session["MissingPair"]))
            {
                if (Session["IndividualEvent"].ToString() == "YES")
                {
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairEW, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameEast"] = tempName;
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.West, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameWest"] = tempName;
                    return View("NSMissingIndividual");
                }
                else
                {
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairEW, "E", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameEast"] = tempName;
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairEW, "W", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameWest"] = tempName;
                    return View("NSMissing");
                }
            }
            else if (round.PairEW == 0 || round.PairEW == Convert.ToInt32(Session["MissingPair"]))
            {
                if (Session["IndividualEvent"].ToString() == "YES")
                {
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairNS, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameNorth"] = tempName;
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.South, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameSouth"] = tempName;
                    return View("EWMissingIndividual");
                }
                else
                {
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairNS, "N", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameNorth"] = tempName;
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairNS, "S", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameSouth"] = tempName;
                    return View("EWMissing");
                }
            }
            else
            {
                if (Session["IndividualEvent"].ToString() == "YES")
                {
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairNS, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameNorth"] = tempName;
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.South, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameSouth"] = tempName;
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairEW, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameEast"] = tempName;
                    tempName = Player.GetNameIndividual(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.West, false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameWest"] = tempName;
                    return View("Individual");
                }
                else
                {
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairNS, "N", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameNorth"] = tempName;
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairNS, "S", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameSouth"] = tempName;
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairEW, "E", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameEast"] = tempName;
                    tempName = Player.GetName(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["CurrentRound"]), round.PairEW, "W", false);
                    if (tempName == "Error") return RedirectToAction("Index", "ErrorScreen");
                    ViewData["PlayerNameWest"] = tempName;
                    return View("Pair");
                }
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult OKSitOutButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList", new { round = Convert.ToInt32(Session["CurrentRound"]), finalRound = "No" });
        }

        public ActionResult BackButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            // Need to reset pair and board numbers
            int previousRound = Convert.ToInt32(Session["CurrentRound"]) - 1;      // Round > 1 else no Back button 
            Round round;
            if (Session["IndividualEvent"].ToString() == "YES")
            {
                round = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), previousRound, true);
                if (round.PairNS == -1) return RedirectToAction("Index", "ErrorScreen");
                Session["PairNS"] = round.PairNS.ToString();
                Session["South"] = round.South.ToString();
                Session["PairEW"] = round.PairEW.ToString();
                Session["West"] = round.West.ToString();
            }
            else
            {
                round = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), previousRound, false);
                if (round.PairNS == -1) return RedirectToAction("Index", "ErrorScreen");
                Session["PairNS"] = round.PairNS.ToString();
                Session["PairEW"] = round.PairEW.ToString();
            }
            Session["LowBoard"] = round.LowBoard;
            Session["HighBoard"] = round.HighBoard;

            return RedirectToAction("Index", "ShowMove", new { newRound = Convert.ToInt32(Session["CurrentRound"]) });
        }
    }
}