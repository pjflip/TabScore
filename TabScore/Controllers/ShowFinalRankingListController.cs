using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowFinalRankingListController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Round round = Session["Round"] as Round;
            SessionData sessionData = Session["SessionData"] as SessionData;

            RankingList rankingList = new RankingList(DBConnectionString, sessionData.SectionID, sessionData.IsIndividual);
            if (rankingList != null && rankingList.Count != 0 && rankingList[0].Score != "     0" && rankingList[0].Score != "50")
            {
                rankingList.PairNS = round.PairNS;
                rankingList.PairEW = round.PairEW;
                ViewData["BackButton"] = "REFRESH";
                if (sessionData.IsIndividual)
                {
                    rankingList.South = round.South;
                    rankingList.West = round.West;
                    return View("Individual", rankingList);
                }
                else if (rankingList.Exists(x => x.Orientation == "E"))
                {
                    return View("TwoWinners", rankingList);
                }
                else
                {
                    return View("OneWinner", rankingList);
                }
            }

            return RedirectToAction("Index", "EndScreen");
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "EndScreen");
        }

        public ActionResult RefreshButtonClick()
        {
            return RedirectToAction("Index", "ShowFinalRankingList");
        }
    }
}