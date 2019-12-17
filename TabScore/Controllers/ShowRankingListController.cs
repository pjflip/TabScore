using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRankingListController : Controller
    {
        public ActionResult Index()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            Round round = Session["Round"] as Round;
            if (round.RoundNumber > 1)  // Show ranking list only from round 2 onwards
            {
               if (new Settings(DBConnectionString).ShowRanking == 1)
                {
                    Sesh sesh = Session["Sesh"] as Sesh;
                    RankingList rankingList = new RankingList(DBConnectionString, sesh.SectionID, sesh.IsIndividual);
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].ScoreDecimal != 0.0 && rankingList[0].ScoreDecimal != 50.0)
                    {
                        rankingList.RoundNumber = round.RoundNumber;
                        rankingList.PairNS = round.PairNS;
                        rankingList.PairEW = round.PairEW;
                        ViewData["BackButton"] = "REFRESH";
                        if (sesh.IsIndividual)
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
                }
            }
            return RedirectToAction("Index", "ShowMove", new { newRoundNumber = round.RoundNumber + 1 });
        }

        public ActionResult OKButtonClick(int roundNumber)   // Pass back round to ensure it is not incremented twice by a double bounce
        {
            return RedirectToAction("Index", "ShowMove", new { newRoundNumber = roundNumber + 1 });
        }
        public ActionResult RefreshButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList");
        }
    }
}