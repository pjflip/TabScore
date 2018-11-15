using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class ControlSectionTableNoPresetController : Controller
    {
        public ActionResult EnterSection()
        {
            if (Request.Cookies["SectionLetter"] != null)
            {
                Session["SectionLetter"] = Request.Cookies["SectionLetter"].Value.ToString();
                ViewData["SectionLetter"] = Session["SectionLetter"];
            }
            else
            {
                ViewData["SectionLetter"] = "unassigned";
            }
 
            ViewBag.Header = "Section Preset";
            ViewData["CancelButton"] = "TRUE";
            return View();
        }

        public ActionResult SectionOKButtonClick()
        {
            return RedirectToAction("EnterTableNo");
        }

        public ActionResult EnterTableNo()
        {
            if (Request.Cookies["TableNo"] != null)
            {
                Session["Table"] = Request.Cookies["TableNo"].Value.ToString();
                ViewData["Table"] = Session["Table"];
            }
            else
            {
                ViewData["Table"] = "unassigned";
            }

            ViewBag.Header = "Table Number Preset";
            ViewData["CancelButton"] = "FALSE";
            return View();
        }

        public ActionResult TableNoOKButtonClick()
        {
            return RedirectToAction("Index", "ControlMenu");
        }

        public ActionResult CancelPreset()
        {
            ViewBag.Header = "Cancel Presets";
            ViewData["CancelButton"] = "TRUE";
            return View();
        }

        public ActionResult CancelPresetOKButtonClick()
        {
            return RedirectToAction("Index", "ControlMenu");
        }

        public ActionResult CancelButtonClick()
        {
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}