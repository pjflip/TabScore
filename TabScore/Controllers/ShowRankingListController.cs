using System;
using System.Collections.Generic;
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

            if (Convert.ToInt32(Session["Round"]) > 1)
            {
                int showRankingSetting = Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking);
                if (showRankingSetting == 1)
                {
                    List<RankingListClass> rankingList = RankingList.GetRankingList(DBConnectionString, Session["SectionID"].ToString());
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].Score != "     0" && rankingList[0].Score != "50")
                    {
                        ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]} - Round {Session["Round"]} - NS {Session["PairNS"]} v EW {Session["PairEW"]}";
                        ViewData["BackButton"] = "REFRESH";
                        if (rankingList.Exists(x => x.Orientation == "E"))
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
            return RedirectToAction("Index", "ShowMove", new { newRound = (Convert.ToInt32(Session["Round"]) + 1).ToString() });
        }

        public ActionResult OKButtonClick(string round)   // Pass back round to ensure it is not incremented twice by a double bounce
        {
            return RedirectToAction("Index", "ShowMove", new { newRound = (Convert.ToInt32(round) + 1).ToString() });
        }
        public ActionResult RefreshButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList");
        }
    }
}