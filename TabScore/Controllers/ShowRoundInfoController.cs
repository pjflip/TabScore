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

            Round round = Session["Round"] as Round;

            if (round.RoundNumber == 1)
            {
                ViewData["BackButton"] = "FALSE";
            }
            else 
            {
                ViewData["BackButton"] = "TRUE";
            }
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            if (round.PairNS == 0 || round.PairNS == Convert.ToInt32(Session["MissingPair"]))
            {
                if (Convert.ToBoolean(Session["IndividualEvent"]))
                {
                   return View("NSMissingIndividual", round);
                }
                else
                {
                    return View("NSMissing", round);
                }
            }
            else if (round.PairEW == 0 || round.PairEW == Convert.ToInt32(Session["MissingPair"]))
            {
                if (Convert.ToBoolean(Session["IndividualEvent"]))
                {
                   return View("EWMissingIndividual", round);
                }
                else
                {
                    return View("EWMissing", round);
                }
            }
            else
            {
                if (Convert.ToBoolean(Session["IndividualEvent"]))
                {
                    return View("Individual", round);
                }
                else
                {
                   return View("Pair", round);
                }
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult OKSitOutButtonClick()
        {
            Round round = Session["Round"] as Round;
            return RedirectToAction("Index", "ShowRankingList", new { roundNumber = round.RoundNumber, finalRound = "No" });
        }

        public ActionResult BackButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            // Reset to the previous round; RoundNumber > 1 else no Back button and cannot get here
            Round round = Session["Round"] as Round;
            Session["Round"] = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), round.RoundNumber - 1, Convert.ToBoolean(Session["IndividualEvent"]));
            return RedirectToAction("Index", "ShowMove", new { newRoundNumber = round.RoundNumber });
        }
    }
}