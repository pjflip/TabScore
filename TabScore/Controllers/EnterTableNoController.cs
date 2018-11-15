using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterTableNoController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Header = $"Section {Session["SectionLetter"]}";
            ViewData["CancelButton"] = "FALSE";
            ViewData["NumTables"] = Session["NumTables"];
            return View();
        }

        public ActionResult OKButtonClick(string tn)
        {
            if (Tables.LogonTable(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), tn))
            {
                Session["Table"] = tn;
                return RedirectToAction("Index", "ShowPlayerNos");
            }
            else
            {
                TempData["alertMessage"] = $"Table {tn} already logged on";
                return RedirectToAction("Index", "EnterTableNo");
            }
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "EnterSection";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}
