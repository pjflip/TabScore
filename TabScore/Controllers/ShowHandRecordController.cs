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
            Section section = Session["Section"] as Section;

            HandRecord handRecord = new HandRecord(DBConnectionString, section.ID, result.BoardNumber);
            if (handRecord.NorthSpades == "###" && section.ID != 1)    // Use default Section 1 hand records)
            {
                handRecord = new HandRecord(DBConnectionString, 1, result.BoardNumber);
            }

            ViewData["BackButton"] = "FALSE";
            return View(handRecord);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowTraveller");
        }
    }
}