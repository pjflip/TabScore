// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowFinalRankingListController : Controller
    {
        public ActionResult Index()
        {
            RankingList rankingList = new RankingList((Session["SessionData"] as SessionData).SectionID);

            // Don't show the ranking list if it doesn't contain anything useful
            if (rankingList == null || rankingList.Count == 0 || rankingList[0].ScoreDecimal == 0 || rankingList[0].ScoreDecimal == 50)
            {
                return RedirectToAction("Index", "EndScreen");
            }
            else
            {
                Round round = Session["Round"] as Round;
                rankingList.PairNS = round.PairNS;
                rankingList.PairEW = round.PairEW;
                ViewData["BackButton"] = "REFRESH";
                if (AppData.IsIndividual)
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
}