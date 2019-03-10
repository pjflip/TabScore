using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = "";
            ViewData["BackButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "StartScreen");
            }

            // Check if new round has been added
            RoundClass round = Round.GetRoundInfo(DBConnectionString, Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString());
            if(round.PairNS == 0)
            {
                return RedirectToAction("Index", "EndScreen");
            }

            // Revert to previous round to pick up move 
            Session["Round"] = (Convert.ToInt32(Session["Round"]) - 1).ToString();
            return RedirectToAction("Index", "ShowMove");
        }
    }
}