using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (!(Session["Settings"] as Settings).ShowResults)
            {
                return RedirectToAction("Index", "ShowBoards");
            }

            Result result = Session["Result"] as Result;
            SessionData sessionData = Session["SessionData"] as SessionData;
            Traveller traveller = new Traveller(DBConnectionString, sessionData.SectionID, result.BoardNumber, result.PairNS, sessionData.IsIndividual, Session["Settings"] as Settings);
            ViewData["BackButton"] = "TRUE";
            if (sessionData.IsIndividual)
            {
                return View("Individual", traveller);
            }
            else
            {
                return View("Pairs", traveller);
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult HandRecordButtonClick()
        {
            return RedirectToAction("Index", "ShowHandRecord");
        }
    }
}
