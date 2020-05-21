// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRankingListController : Controller
    {
        public ActionResult Index()
        {
            Round round = Session["Round"] as Round;
            if (round.RoundNumber > 1)  // Show ranking list only from round 2 onwards
            {
                if (Settings.ShowRanking == 1)
                {
                    RankingList rankingList = new RankingList((Session["SessionData"] as SessionData).SectionID);
                    
                    // Only show the ranking list if it contains something meaningful
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].ScoreDecimal != 0 && rankingList[0].ScoreDecimal != 50)
                    {
                        rankingList.RoundNumber = round.RoundNumber;
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
            return RedirectToAction("Index", "ShowMove", new { newRoundNumber = round.RoundNumber + 1 });
        }
    }
}