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
            int roundNumber = (Session["Round"] as Round).RoundNumber;
            if (roundNumber == UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, (Session["Sesh"] as Sesh).SectionID))  
            {
                // Final round, so no new rounds added
                return RedirectToAction("Index", "EndScreen");
            }
            else
            {
                return RedirectToAction("Index", "ShowMove", new { newRoundNumber = roundNumber + 1 });
            }
        }
    }
}