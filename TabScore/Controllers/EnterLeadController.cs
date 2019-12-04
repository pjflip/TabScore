using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterLeadController : Controller
    {
        public ActionResult Index(string secondPass)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (!Settings.GetSetting<bool>(DBConnectionString, SettingName.EnterLeadCard))
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }

            Result result = Session["Result"] as Result;

            ViewData["BackButton"] = "TRUE";
            ViewData["SecondPass"] = secondPass;
            return View(result);
        }

        public ActionResult OKButtonClick(string card, string secondPass)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Result result = Session["Result"] as Result;
            if (!Settings.GetSetting<bool>(DBConnectionString, SettingName.ValidateLeadCard) || secondPass == "TRUE" || UtilityFunctions.ValidateLead(DBConnectionString, result.SectionID, result.Board, card, result.NSEW))
            {
                result.LeadCard = card;
                Session["Result"] = result;
                return RedirectToAction("Index", "EnterTricksTaken");
            }
            else
            {
                return RedirectToAction("Index", "EnterLead", new { secondPass = "TRUE" });
            }
        }

        public ActionResult BackButtonClick()
        {
            Result result = Session["Result"] as Result;
            return RedirectToAction("Index", "EnterContract", new { board = result.Board } );
        }
    }
}