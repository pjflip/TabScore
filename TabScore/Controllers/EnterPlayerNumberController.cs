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
            Session["Header"] = $"Table {(Session["Section"] as Section).Letter}{Session["TableNumber"]}";
            return View();
        }

        public ActionResult OKButtonClick(string direction, int playerNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            new Player(DBConnectionString, (Session["Section"] as Section).ID, Convert.ToInt32(Session["TableNumber"]), Session["Round"] as Round, direction, playerNumber, Convert.ToBoolean(Session["IndividualEvent"])).UpdateDatabase();
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}