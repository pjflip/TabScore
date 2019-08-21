using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = $"Section {Session["SectionLetter"]}";
            ViewData["BackButton"] = "FALSE";
            ViewData["NumTables"] = Session["NumTables"];
            return View();
        }

        public ActionResult OKButtonClick(string tableNumber, string confirm)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            int logonStatus = Tables.IsLoggedOn(DBConnectionString, Session["SectionID"].ToString(), tableNumber);
            if (logonStatus == -1) return RedirectToAction("Index", "ErrorScreen");
            if (confirm == "TRUE" || (logonStatus == 2))     // Status=2 means table not logged on
            {
                Session["Table"] = tableNumber;
                if (Tables.Logon(DBConnectionString, Session["SectionID"].ToString(), tableNumber) == -1) return RedirectToAction("Index", "ErrorScreen");

                // Check if results data exist - set round number accordingly
                int round = Round.GetLastEnteredRound(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString());
                if (round == -1) return RedirectToAction("Index", "ErrorScreen");

                Session["Round"] = round.ToString();
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
