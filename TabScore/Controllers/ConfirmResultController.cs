using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ConfirmResultController : Controller
    {
        public ActionResult Index()
        {
            Result result = Session["Result"] as Result;
            result.CalculateScore();
            Session["Result"] = result;

            ViewData["BackButton"] = "TRUE";
            return View(result);
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Result result = Session["Result"] as Result;
            result.WriteToDB(DBConnectionString, Convert.ToBoolean(Session["IndividualEvent"]));
            return RedirectToAction("Index", "ShowTraveller");
        }

        public ActionResult BackButtonClick()
        {
            Result result = Session["Result"] as Result;
            if (result.ContractLevel == 0)  // This was passed out, so Back goes all the way to Enter Contract screen
            {
                return RedirectToAction("Index", "EnterContract", new { board = result.Board });
            }
            else
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }
        }
    }
}
