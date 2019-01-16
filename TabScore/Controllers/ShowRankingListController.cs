using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRankingListController : Controller
    {
        public ActionResult Index(string finalRound)
        {
            if (Convert.ToInt32(Session["Round"]) > 1)
            {
                int showRankingSetting = Settings.ShowRanking(Session["DBConnectionString"].ToString());
                if (showRankingSetting == 1 || (showRankingSetting == 2 && finalRound == "Yes"))
                {
                    List<RankingListClass> rankingList = RankingList.GetRankingList(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString());
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].Score != "     0")
                    {
                        ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
                        ViewData["BackButton"] = "FALSE";
                        ViewData["PairNS"] = Session["PairNS"];
                        ViewData["PairEW"] = Session["PairEW"];
                        bool twoWinners = rankingList.Exists(x => x.Orientation == "E");
                        if (twoWinners)
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
            if (finalRound == "Yes")
            {
                return RedirectToAction("Index", "EndScreen");
            }
            else
            {
                return RedirectToAction("Index", "ShowMove");
            }
        }

        public ActionResult OKButtonClick()
        {
            if (Settings.ShowRanking(Session["DBConnectionString"].ToString()) == 2)   // Can only get here if finalRound = "Yes"
            {
                return RedirectToAction("Index", "EndScreen");
            }
            else
            {
                return RedirectToAction("Index", "ShowMove");
            }
        }
    }
}