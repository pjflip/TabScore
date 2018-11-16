using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowHandRecordController : Controller
    {
        public ActionResult Index()
        {
            HandRecordClass hr = HandRecord.GetHandRecord(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Board"].ToString());

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["CancelButton"] = "FALSE";
            ViewData["Board"] = Session["Board"];
            ViewData["PairNS"] = Session["PairNS"];
            int board = Convert.ToInt32(Session["Board"]);
            switch ((board - 1) % 4)
            {
                case 0:
                    ViewData["Dealer"] = "N";
                    break;
                case 1:
                    ViewData["Dealer"] = "E";
                    break;
                case 2:
                    ViewData["Dealer"] = "S";
                    break;
                case 3:
                    ViewData["Dealer"] = "W";
                    break;
            }
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