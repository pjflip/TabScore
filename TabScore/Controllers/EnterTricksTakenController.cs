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
            if (new Settings(DBConnectionString).EnterResultsMethod == 1)
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

            if (new Settings(DBConnectionString).EnterLeadCard)
            {
                return RedirectToAction("Index", "EnterLead", new { validateWarning = "NoWarning" });
            }
            else
            {
                Result result = Session["Result"] as Result;
                return RedirectToAction("Index", "EnterContract", new { boardNumber = result.BoardNumber });
            }
        }
    }
}