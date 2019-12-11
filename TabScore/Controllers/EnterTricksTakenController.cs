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

            ViewData["BackButton"] = "TRUE";
            if (new Settings(DBConnectionString).EnterResultsMethod == 1)
            {
                return View("TotalTricks", Session["Result"] as Result);
            }
            else
            {
                return View("TricksPlusMinus", Session["Result"] as Result);
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
                return RedirectToAction("Index", "EnterContract", new { boardNumber = (Session["Result"] as Result).BoardNumber });
            }
        }
    }
}