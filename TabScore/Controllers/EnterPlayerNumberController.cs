using TabScore.Models;
using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
       
        public ActionResult Index(string dir)
        {
            ViewData["Direction"] = dir;
            ViewData["BackButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            return View();
        }

        public ActionResult OKButtonClick(string direction, string playerNumber)
        {
            Player.UpdateDatabase(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), Session["Round"].ToString(), direction, playerNumber);

            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}