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
            Sesh sesh = Session["Sesh"] as Sesh;
            Session["Header"] = $"Table {sesh.SectionTableString}";
            return View();
        }

        public ActionResult OKButtonClick(string direction, int playerNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Sesh sesh = Session["Sesh"] as Sesh;
            new Player(DBConnectionString, sesh.SectionID, sesh.TableNumber, Session["Round"] as Round, direction, playerNumber, sesh.IsIndividual).UpdateDatabase();
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}