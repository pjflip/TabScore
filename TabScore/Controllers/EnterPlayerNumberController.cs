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

            if (Player.UpdateDatabase(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), Convert.ToInt32(Session["CurrentRound"]), direction, playerNumber, Session["IndividualEvent"].ToString() == "YES") == "Error")
            {
                return RedirectToAction("Index", "ErrorScreen");
            };
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}