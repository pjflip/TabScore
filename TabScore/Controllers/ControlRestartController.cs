using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ControlRestartController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = "Restart";
            ViewData["CancelButton"] = "TRUE";
            return View();
        }

        public ActionResult OKButtonClick()
        {
            if(Session["DBConnectionString"].ToString() != "" && Session["Table"].ToString() != "")
            {
                Tables.LogoffTable(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString());
            }
            return RedirectToAction("Index", "StartScreen");
        }

        public ActionResult CancelButtonClick()
        {
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}