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

            Sesh sesh = Session["Sesh"] as Sesh;
            if (newRoundNumber > UtilityFunctions.NumberOfRoundsInEvent(DBConnectionString, sesh.SectionID))  // Session complete
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

            MovesList movesList = new MovesList(DBConnectionString, sesh.SectionID, Session["Round"] as Round, newRoundNumber, sesh.TableNumber, sesh.MissingPair, sesh.IsIndividual);

            Session["Header"] = $"Table {sesh.SectionTableString}";
            ViewData["BackButton"] = "FALSE";

            // Update session to new round info
            Session["Round"] = new Round(DBConnectionString, sesh.SectionID, sesh.TableNumber, newRoundNumber, sesh.IsIndividual);

            if (sesh.IsIndividual)
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