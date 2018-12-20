using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNumberController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = $"Section {Session["SectionLetter"]}";
            ViewData["CancelButton"] = "FALSE";
            ViewData["NumTables"] = Session["NumTables"];
            return View();
        }

        public ActionResult OKButtonClick(string tableNumber, string confirm)
        {
            if (confirm == "TRUE" || !Tables.IsLoggedOn(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tableNumber))
            {
                Session["Table"] = tableNumber;
                Tables.Logon(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tableNumber);

                // Check if results data exist - set round number accordingly
                int round = Round.GetLastEnteredRound(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString());
                Session["Round"] = round.ToString();
                if (round == 1 || Settings.NumberEntryEachRound(Session["DBConnectionString"].ToString()))
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
