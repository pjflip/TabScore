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

            if (!new Settings(DBConnectionString).ShowResults)
            {
                return RedirectToAction("Index", "ShowBoards");
            }

            Result result = Session["Result"] as Result;
            Sesh sesh = Session["Sesh"] as Sesh;
            Traveller traveller = new Traveller(DBConnectionString, sesh.SectionID, result.BoardNumber, result.PairNS, sesh.IsIndividual);
            ViewData["BackButton"] = "TRUE";
            if (sesh.IsIndividual)
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
