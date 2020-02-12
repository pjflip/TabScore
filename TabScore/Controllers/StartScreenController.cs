using System.Reflection;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class StartScreenController : Controller
    {
        public ActionResult Index()
        {
            Session["Header"] = "";
            ViewData["BackButton"] = "FALSE";
            ViewData["Version"]= Assembly.GetExecutingAssembly().GetName().Version.ToString();
            return View();
        }

        public ActionResult OKButtonClick()
        {
            AppData.Refresh();
            if (AppData.DBConnectionString == "")
            {
                TempData["warningMessage"] = "Scoring database not yet started";
                return RedirectToAction("Index", "StartScreen");
            }
            Settings.Refresh();

            return RedirectToAction("Index", "EnterSection");
        }
    }
}