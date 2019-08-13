using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowHandRecordController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "")
            {
                return RedirectToAction("Index", "ErrorScreen");
            }

            HandRecordClass hr = HandRecord.GetHandRecord(DBConnectionString, Session["SectionID"].ToString(), Session["Board"].ToString());
            if (hr.NorthSpades == "###" && Session["SectionID"].ToString() != "1")    // Use default Section 1 hand records)
            {
               hr = HandRecord.GetHandRecord(DBConnectionString, "1", Session["Board"].ToString());
            }

            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - {Vulnerability.SetPairString("NS", Session["Board"].ToString(), Session["PairNS"].ToString())} v {Vulnerability.SetPairString("EW", Session["Board"].ToString(), Session["PairEW"].ToString())}";
            ViewData["BackButton"] = "FALSE";
            ViewData["Dealer"] = Dealer.GetDealerForBoard(Session["Board"].ToString());
            return View(hr);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowTraveller");
        }
    }
}