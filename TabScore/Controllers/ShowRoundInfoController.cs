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

            Section section = Session["Section"] as Section;
            Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]}";

            if (round.PairNS == 0 || round.PairNS == section.MissingPair)
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
            else if (round.PairEW == 0 || round.PairEW == section.MissingPair)
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
            return RedirectToAction("Index", "ShowRankingList");
        }

        public ActionResult BackButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            // Reset to the previous round; RoundNumber > 1 else no Back button and cannot get here
            int roundNumber =(Session["Round"] as Round).RoundNumber;
            Session["Round"] = new Round(DBConnectionString, (Session["Section"] as Section).ID, Convert.ToInt32(Session["TableNumber"]), roundNumber - 1, Convert.ToBoolean(Session["IndividualEvent"]));
            return RedirectToAction("Index", "ShowMove", new { newRoundNumber = roundNumber });
        }
    }
}