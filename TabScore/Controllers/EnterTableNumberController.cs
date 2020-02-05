using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index() 
        {
            SessionData sessionData = Session["SessionData"] as SessionData;
            Session["Header"] = $"Section {sessionData.SectionLetter}";
            ViewData["BackButton"] = "FALSE"; 
            return View(sessionData);   // At this stage, TableNumber > 0 means we've already tried to log on and need to confirm
        }

        public ActionResult OKButtonClick(int tableNumber, string confirm)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            SessionData sessionData = Session["SessionData"] as SessionData;
            sessionData.TableNumber = tableNumber;

            if (confirm == "FALSE")
            {
                if (sessionData.TableLogonStatus(DBConnectionString) == 1)  // Table is already logged on, so need to confirm
                {
                    Session["SessionData"] = sessionData;   // Save table number
                    return RedirectToAction("Index", "EnterTableNumber");
                }
                else
                {
                    sessionData.LogonTable(DBConnectionString);
                }
            }

            sessionData.SectionTableString = sessionData.SectionLetter + tableNumber.ToString();   // Concatenate valid table number to section letter to give eg "A1", and save
            Session["SessionData"] = sessionData;    
            
            // Check if results data exist - set round number accordingly and create round info
            int lastRoundWithResults = UtilityFunctions.GetLastRoundWithResults(DBConnectionString, sessionData.SectionID, tableNumber);
            Session["Round"] = new Round(DBConnectionString, sessionData.SectionID, tableNumber, lastRoundWithResults, sessionData.IsIndividual);

            if (lastRoundWithResults == 1 || (Session["Settings"] as Settings).NumberEntryEachRound)
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