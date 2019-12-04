using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index()
        {
            Session["Header"] = "";
            ViewData["BackButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")return RedirectToAction("Index", "ErrorScreen");

            // Check if new round has been added; can't apply to individuals
            int maxRounds = UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, Convert.ToInt32(Session["SectionID"]));
            Round round = Session["Round"] as Round;
            if (round.RoundNumber == maxRounds)  // Final round, so no new rounds added
            {
                 return RedirectToAction("Index", "EndScreen");
            }
            else
            {
                Session["MaxRounds"] = maxRounds;
                return RedirectToAction("Index", "ShowMove", new { newRoundNumber = round.RoundNumber + 1 });
            }
        }
    }
}