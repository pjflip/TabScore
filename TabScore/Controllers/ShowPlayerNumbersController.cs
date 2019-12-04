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

            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {round.RoundNumber}";

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
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}