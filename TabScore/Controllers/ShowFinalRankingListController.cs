using System;
using System.Collections.Generic;
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