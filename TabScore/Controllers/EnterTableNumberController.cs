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

            int logonStatus = Tables.IsLoggedOn(DBConnectionString, Convert.ToInt32(Session["SectionID"]), tableNumber);
            if (logonStatus == -1) return RedirectToAction("Index", "ErrorScreen");
            if (confirm == "TRUE" || (logonStatus == 2))     // Status=2 means table not logged on
            {
                Session["Table"] = tableNumber;
                if (Tables.Logon(DBConnectionString, Convert.ToInt32(Session["SectionID"]), tableNumber) == -1) return RedirectToAction("Index", "ErrorScreen");

                // Check if results data exist - set round number accordingly
                int round = LastRoundEntered.Get(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]));
                if (round == -1) return RedirectToAction("Index", "ErrorScreen");

                Session["CurrentRound"] = round.ToString();
                if (round == 1 || Settings.GetSetting<bool>(DBConnectionString, SettingName.NumberEntryEachRound))
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
