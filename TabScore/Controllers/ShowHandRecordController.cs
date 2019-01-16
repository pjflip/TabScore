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
            if (hr.NorthSpades == "###" && Session["SectionID"].ToString() != "1")    // Use default Section 1 hand records)
            {
               hr = HandRecord.GetHandRecord(Session["DBConnectionString"].ToString(), "1", Session["Board"].ToString());
            }

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["BackButton"] = "FALSE";
            ViewData["Board"] = Session["Board"];
            ViewData["Dealer"] = Dealer.GetDealerForBoard(Session["Board"].ToString());
            return View(hr);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowTraveller");
        }
    }
}