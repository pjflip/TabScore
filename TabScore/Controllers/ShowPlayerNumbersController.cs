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
            Sesh sesh = Session["Sesh"] as Sesh;

            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {sesh.SectionTableString} - Round {round.RoundNumber}";

            if (round.PairNS == 0 || round.PairNS == sesh.MissingPair)
            {
                if (sesh.IsIndividual)
                {
                    return View("NSMissingIndividual", round);
                }
                else
                {
                    return View("NSMissing", round);
                }
            }
            else if (round.PairEW == 0 || round.PairEW == sesh.MissingPair)
            {
                if (sesh.IsIndividual)
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
                if (sesh.IsIndividual)
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