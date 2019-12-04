using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
        public ActionResult Index(string dir)
        {
            ViewData["Direction"] = dir;
            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]}";
            return View();
        }

        public ActionResult OKButtonClick(string direction, string playerNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Round round = Session["Round"] as Round;
            PlayerFunctions.UpdateDatabase(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), round.RoundNumber, direction, playerNumber, Convert.ToBoolean(Session["IndividualEvent"]));
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}