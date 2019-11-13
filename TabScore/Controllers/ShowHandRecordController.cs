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
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            HandRecord hr = new HandRecord(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Board"]));
            if (hr.NorthSpades == "Error") return RedirectToAction("Index", "ErrorScreen");
            if (hr.NorthSpades == "###" && Convert.ToInt32(Session["SectionID"]) != 1)    // Use default Section 1 hand records)
            {
                hr = new HandRecord(DBConnectionString, 1, Convert.ToInt32(Session["SectionID"]));
            }

            ViewData["BackButton"] = "FALSE";
            ViewData["Dealer"] = Dealer.GetDealerForBoard(Convert.ToInt32(Session["Board"]));
            return View(hr);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowTraveller");
        }
    }
}