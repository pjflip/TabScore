using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int newRoundNumber)
        {
            SessionData sessionData = Session["SessionData"] as SessionData;
            if (newRoundNumber > UtilityFunctions.NumberOfRoundsInEvent(sessionData.SectionID))  // Session complete
            {
                if (Settings.ShowRanking == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList");
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen");
                }
            }

            MovesList movesList = new MovesList(sessionData, Session["Round"] as Round, newRoundNumber);

            Session["Header"] = $"Table {sessionData.SectionTableString}";
            ViewData["BackButton"] = "FALSE";

            if (AppData.IsIndividual)
            {
                return View("Individual", movesList);
            }
            else
            {
                return View("Pair", movesList);
            }
        }

        public ActionResult OKButtonClick(int newRoundNumber)
        {
            // Refresh settings for the start of the round
            Settings.Refresh();

            // Update session to new round info
            Session["Round"] = new Round(Session["SessionData"] as SessionData, newRoundNumber);

            if (Settings.NumberEntryEachRound)
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