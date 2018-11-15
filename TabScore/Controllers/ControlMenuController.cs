using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class ControlMenuController : Controller
    {
        public ActionResult Index()
        {
            ViewData["CancelButton"] = "FALSE";
            ViewBag.Header = $"TabScore Control Menu";

            return View();
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", Session["ControlReturnScreen"].ToString());
        }

        public ActionResult CancelButtonClick()
        {
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}