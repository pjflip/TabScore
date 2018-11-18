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
                SettingsClass settings = Settings.GetSettings(Session["DBConnectionString"].ToString());
                if (settings.ShowRanking == 1 || (settings.ShowRanking == 2 && finalRound == "Yes"))
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
            SettingsClass settings = Settings.GetSettings(Session["DBConnectionString"].ToString());
            if (settings.ShowRanking == 2)   // Can only get here if finalRound = "Yes"
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