using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTricksTakenController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Result result = Session["Result"] as Result;

            ViewData["BackButton"] = "TRUE";
            if (Settings.GetSetting<int>(DBConnectionString, SettingName.EnterResultsMethod) == 1)
            {
                return View("TotalTricks", result);
            }
            else
            {
                return View("TricksPlusMinus", result);
            }
        }

        public ActionResult OKButtonClick(string numTricks)
        {
            Result result = Session["Result"] as Result;
            result.TricksTakenNumber = Convert.ToInt32(numTricks);
            Session["Result"] = result;
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult BackButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.EnterLeadCard))
            {
                return RedirectToAction("Index", "EnterLead", new { secondPass = "FALSE" });
            }
            else
            {
                Result result = Session["Result"] as Result;
                return RedirectToAction("Index", "EnterContract", new { board = result.Board });
            }
        }
    }
}