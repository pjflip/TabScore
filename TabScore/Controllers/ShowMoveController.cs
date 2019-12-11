using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int newRoundNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Section section = Session["Section"] as Section;
            if (newRoundNumber > UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, section.ID))  // Session complete
            {
                if (new Settings(DBConnectionString).ShowRanking == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList");
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen");
                }
            }

            int tableNumber = Convert.ToInt32(Session["TableNumber"]);
            bool individual = Convert.ToBoolean(Session["IndividualEvent"]);
            MovesList movesList = new MovesList(DBConnectionString, section.ID, Session["Round"] as Round, newRoundNumber, tableNumber, section.MissingPair, individual);

            Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]}";
            ViewData["BackButton"] = "FALSE";

            // Update session to new round info
            Session["Round"] = new Round(DBConnectionString, section.ID, tableNumber, newRoundNumber, individual);

            if (individual)
            {
                return View("Individual", movesList);
            }
            else
            {
                return View("Pair", movesList);
            }
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (new Settings(DBConnectionString).NumberEntryEachRound)
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