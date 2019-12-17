using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            // Reset default session value as no board has yet been selected
            Session["Result"] = null;

            Round round = Session["Round"] as Round;
            Sesh sesh = Session["Sesh"] as Sesh;

            ResultsList resultsList = new ResultsList(DBConnectionString, sesh.SectionID, sesh.TableNumber, round.RoundNumber, round.LowBoard, round.HighBoard);

            if (sesh.IsIndividual)
            {
                Session["Header"] = $"Table {sesh.SectionTableString} - Round {round.RoundNumber} - {round.PairNS}+{round.South} v {round.PairEW}+{round.West}";
            }
            else
            {
                Session["Header"] = $"Table {sesh.SectionTableString} - Round {round.RoundNumber} - NS {round.PairNS} v EW {round.PairEW}";
            }
            ViewData["BackButton"] = "FALSE";

            return View(resultsList);
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList");
        }
    }
}