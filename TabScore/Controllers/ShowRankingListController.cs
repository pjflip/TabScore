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

            if (Convert.ToInt32(Session["CurrentRound"]) > 1)
            {
                int showRankingSetting = Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking);
                if (showRankingSetting == 1)
                {
                    bool individual = Session["IndividualEvent"].ToString() == "YES";
                    List<Ranking> rankingList = RankingList.GetRankingList(DBConnectionString, Convert.ToInt32(Session["SectionID"]), individual);
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].Score != "     0" && rankingList[0].Score != "50")
                    {
                        ViewData["BackButton"] = "REFRESH";
                        if (individual)
                        {
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
            return RedirectToAction("Index", "ShowMove", new { newRound = (Convert.ToInt32(Session["CurrentRound"]) + 1).ToString() });
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