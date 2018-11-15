using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = "";
            ViewData["CancelButton"] = "FALSE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "StartScreen");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "EndScreen";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}