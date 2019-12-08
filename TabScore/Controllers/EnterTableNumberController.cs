using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index()
        {
            Section section = Session["Section"] as Section;
            Session["Header"] = $"Section {section.Letter}";
            TempData["NumTables"] = section.Tables;
            ViewData["BackButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick(string tableNumber, string confirm)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Section section = Session["Section"] as Section;
            Table table = new Table(DBConnectionString, section.ID, Convert.ToInt32(tableNumber));
            if (confirm == "TRUE" || (table.LogonStatus == 2))     // Status=2 means table not logged on
            {
                Session["TableNumber"] = tableNumber;
                table.Logon(DBConnectionString);

                // Check if results data exist - set round number accordingly and create round infp
                int lastRoundWithResults = UtilityFunctions.GetLastRoundWithResults(DBConnectionString, section.ID, Convert.ToInt32(Session["TableNumber"]));
                Session["Round"] = new Round(DBConnectionString, section.ID, Convert.ToInt32(Session["TableNumber"]), lastRoundWithResults, Convert.ToBoolean(Session["IndividualEvent"]));

                if (lastRoundWithResults == 1 || new Settings(DBConnectionString).NumberEntryEachRound)
                {
                    return RedirectToAction("Index", "ShowPlayerNumbers");
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo");
                }
            }
            else  // Table already logged on - confirm log on anyway?
            {
                TempData["LoggedOnTable"] = tableNumber;
                return RedirectToAction("Index", "EnterTableNumber");
            }
        }
    }
}