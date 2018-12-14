using TabScore.Models;
using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
       
        public ActionResult Index(string dir)
        {
            ViewData["Direction"] = dir;
            ViewData["CancelButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            return View();
        }

        public ActionResult OKButtonClick(string direction, string playerNumber)
        {
            PlayerNumber.UpdateDB(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), direction, playerNumber);

            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}