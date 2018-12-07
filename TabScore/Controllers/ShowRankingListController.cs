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
                    if (rankingList != null)
                    {
                        ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
                        ViewData["CancelButton"] = "FALSE";
                        ViewData["PairNS"] = Session["PairNS"];
                        ViewData["PairEW"] = Session["PairEW"];
                        if (Session["Winners"].ToString() == "1")
                        {
                            return View("OneWinner", rankingList);
                        }
                        else if (Session["Winners"].ToString() == "2")
                        {
                            return View("TwoWinners", rankingList);
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