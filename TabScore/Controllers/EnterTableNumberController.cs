using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index()
        {
            Session["Header"] = $"Section {Session["SectionLetter"]}";
            ViewData["BackButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick(string tableNumber, string confirm)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Table table = new Table(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(tableNumber));
            if (confirm == "TRUE" || (table.LogonStatus == 2))     // Status=2 means table not logged on
            {
                Session["Table"] = tableNumber;
                table.Logon(DBConnectionString);

                // Check if results data exist - set round number accordingly and create round infp
                int lastRoundWithResults = UtilityFunctions.GetLastRoundWithResults(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]));
                Session["Round"] = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), lastRoundWithResults, Convert.ToBoolean(Session["IndividualEvent"]));

                if (lastRoundWithResults == 1 || Settings.GetSetting<bool>(DBConnectionString, SettingName.NumberEntryEachRound))
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