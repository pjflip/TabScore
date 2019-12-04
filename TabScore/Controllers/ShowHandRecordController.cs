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

            Result result = Session["Result"] as Result;
            HandRecord hr = new HandRecord(DBConnectionString, Convert.ToInt32(Session["SectionID"]), result.Board);
            if (hr.NorthSpades == "###" && Convert.ToInt32(Session["SectionID"]) != 1)    // Use default Section 1 hand records)
            {
                hr = new HandRecord(DBConnectionString, 1, Convert.ToInt32(Session["SectionID"]));
            }

            ViewData["BackButton"] = "FALSE";
            return View(hr);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowTraveller");
        }
    }
}