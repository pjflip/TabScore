using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index()
        {
            Session["Header"] = "";
            ViewData["BackButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")return RedirectToAction("Index", "ErrorScreen");

            // Check if new round has been added; can't apply to individuals
            int maxRounds = DBInfo.MaxRounds(DBConnectionString, Convert.ToInt32(Session["SectionID"]));
            if (maxRounds == -1) return RedirectToAction("Index", "ErrorScreen");
            if (Convert.ToInt32(Session["CurrentRound"]) > maxRounds)  // No New rounds added
            {
                    return RedirectToAction("Index", "EndScreen");
            }
            else
            {
                return RedirectToAction("Index", "ShowMove", new { newRound = Convert.ToInt32(Session["CurrentRound"]) });
            }
        }
    }
}