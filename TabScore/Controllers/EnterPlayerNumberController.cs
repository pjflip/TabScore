using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
        public ActionResult Index(string direction)
        {
            ViewData["Direction"] = direction;
            ViewData["BackButton"] = "FALSE";
            SessionData sessionData = Session["SessionData"] as SessionData;
            Session["Header"] = $"Table {sessionData.SectionTableString}";
            return View();
        }

        public ActionResult OKButtonClick(string direction, int playerNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            SessionData sessionData = Session["SessionData"] as SessionData;
            new Player(DBConnectionString, sessionData.SectionID, sessionData.TableNumber, Session["Round"] as Round, direction, playerNumber, sessionData.IsIndividual, (Session["Settings"] as Settings).NameSource).UpdateDatabase();
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}