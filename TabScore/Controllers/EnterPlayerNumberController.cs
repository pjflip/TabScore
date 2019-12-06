using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
        public ActionResult Index(string direction)
        {
            ViewData["Direction"] = direction;
            ViewData["BackButton"] = "FALSE";
            Section section = Session["Section"] as Section;
            Session["Header"] = $"Table {section.Letter}{Session["TableNumber"]}";
            return View();
        }

        public ActionResult OKButtonClick(string direction, string playerNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Round round = Session["Round"] as Round;
            Section section = Session["Section"] as Section;

            PlayerFunctions.UpdateDatabase(DBConnectionString, section.ID, Convert.ToInt32(Session["TableNumber"]), round.RoundNumber, direction, playerNumber, Convert.ToBoolean(Session["IndividualEvent"]));
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}