using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowHandRecordController : Controller
    {
        public ActionResult Index()
        {
            HandRecordClass hr = HandRecord.GetHandRecord(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Board"].ToString());

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
            ViewData["CancelButton"] = "FALSE";
            ViewData["Board"] = Session["Board"];
            ViewData["PairNS"] = Session["PairNS"];
            return View(hr);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowTraveller");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "ShowTraveller";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}