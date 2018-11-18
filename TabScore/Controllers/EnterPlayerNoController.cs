using TabScore.Models;
using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class EnterPlayerNoController : Controller
    {
       
        public ActionResult Index(string dir)
        {
            ViewData["Direction"] = dir;
            ViewData["CancelButton"] = "FALSE";
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";

            return View();
        }

        public ActionResult OKButtonClick(string dir, string PID)
        {
            string Name;
            if (PID == "Unknown")
            {
                Name = PID;
                PID = "0";
            }
            else
            {
                Name = PlayerID.GetNameFromPID(Session["DBConnectionString"].ToString(), PID); 
            }

            dir = dir.Substring(0, 1);
            PlayerNumbers.UpdateNameNumber(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Table"].ToString(), dir, PID, Name);

            return RedirectToAction("Index", "ShowPlayerNos");
        }
    }
}