using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index() 
        {
            Sesh sesh = Session["Sesh"] as Sesh;
            Session["Header"] = $"Section {sesh.SectionLetter}";
            ViewData["BackButton"] = "FALSE"; 
            return View(sesh);   // At this stage, TableNumber > 0 means we've already tried to log on and need to confirm
        }

        public ActionResult OKButtonClick(int tableNumber, string confirm)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Sesh sesh = Session["Sesh"] as Sesh;
            sesh.TableNumber = tableNumber;

            if (confirm == "FALSE")
            {
                if (sesh.TableLogonStatus(DBConnectionString) == 1)  // Table is already logged on, so need to confirm
                {
                    Session["Sesh"] = sesh;   // Save table number
                    return RedirectToAction("Index", "EnterTableNumber");
                }
                else
                {
                    sesh.LogonTable(DBConnectionString);
                }
            }

            sesh.SectionTableString = sesh.SectionLetter + tableNumber.ToString();   // Concatenate valid table number to section letter to give eg "A1", and save
            Session["Sesh"] = sesh;    
            
            // Check if results data exist - set round number accordingly and create round info
            int lastRoundWithResults = UtilityFunctions.GetLastRoundWithResults(DBConnectionString, sesh.SectionID, tableNumber);
            Session["Round"] = new Round(DBConnectionString, sesh.SectionID, tableNumber, lastRoundWithResults, sesh.IsIndividual);

            if (lastRoundWithResults == 1 || new Settings(DBConnectionString).NumberEntryEachRound)
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