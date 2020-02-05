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

            SessionData sessionData = Session["SessionData"] as SessionData;
            if (newRoundNumber > UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, sessionData.SectionID))  // Session complete
            {
                if ((Session["Settings"] as Settings).ShowRanking == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList");
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen");
                }
            }

            MovesList movesList = new MovesList(DBConnectionString, sessionData.SectionID, Session["Round"] as Round, newRoundNumber, sessionData.TableNumber, sessionData.MissingPair, sessionData.IsIndividual);

            Session["Header"] = $"Table {sessionData.SectionTableString}";
            ViewData["BackButton"] = "FALSE";

            // Update session to new round info
            Session["Round"] = new Round(DBConnectionString, sessionData.SectionID, sessionData.TableNumber, newRoundNumber, sessionData.IsIndividual);

            if (sessionData.IsIndividual)
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

            // Refresh settings for the start of the round
            Settings settings = new Settings(DBConnectionString);
            Session["Settings"] = settings;
            
            if (settings.NumberEntryEachRound)
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