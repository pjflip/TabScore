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

            Round round = Session["Round"] as Round;
            Section section = Session["Section"] as Section;

            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]} - Round {round.RoundNumber}";

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
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}