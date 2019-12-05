using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            // Reset default session value as no board has yet been selected
            Session["Result"] = null;

            Round round = Session["Round"] as Round;
            ResultsList resultsList = new ResultsList(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), round.RoundNumber, round.LowBoard, round.HighBoard);

            if (Convert.ToBoolean(Session["IndividualEvent"]))
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {round.RoundNumber} - {round.PairNS}+{round.South} v {round.PairEW}+{round.West}";
            }
            else
            {
                Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {round.RoundNumber} - NS {round.PairNS} v EW {round.PairEW}";
            }
            ViewData["BackButton"] = "FALSE";

            return View(resultsList);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList");
        }
    }
}