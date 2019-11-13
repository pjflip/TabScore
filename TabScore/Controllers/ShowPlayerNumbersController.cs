using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
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
                ViewData["North"] = round.PairNS.ToString();
                ViewData["South"] = round.South.ToString();
                ViewData["East"] = round.PairEW.ToString();
                ViewData["West"] = round.West.ToString();
            }
            else
            {
                round = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), Convert.ToInt32(Session["CurrentRound"]), false);
                if (round.PairNS == -1) return RedirectToAction("Index", "ErrorScreen");
                ViewData["PairNS"] = round.PairNS.ToString();
                ViewData["PairEW"] = round.PairEW.ToString();
            }

            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Convert.ToInt32(Session["CurrentRound"])}";

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
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}