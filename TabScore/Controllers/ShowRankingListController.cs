// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRankingListController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            if (tabletDeviceStatus.RoundNumber > 1)  // Show ranking list only from round 2 onwards
            {
                if (Settings.ShowRanking == 1)
                {
                    RankingList rankingList = new RankingList(tabletDeviceNumber);
                    
                    // Only show the ranking list if it contains something meaningful
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].ScoreDecimal != 0 && rankingList[0].ScoreDecimal != 50)
                    {
                        ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tabletDeviceStatus.RoundNumber}";
                        ViewData["Title"] = $"Ranking List - {tabletDeviceStatus.Location}";
                        ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                        ViewData["TimerSeconds"] = -1;
                        if (AppData.IsIndividual)
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
            return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = tabletDeviceStatus.RoundNumber + 1 });
        }

        public ActionResult Final(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            RankingList rankingList = new RankingList(tabletDeviceNumber);

            // Only show the ranking list if it contains something meaningful
            if (rankingList == null || rankingList.Count == 0 || rankingList[0].ScoreDecimal == 0 || rankingList[0].ScoreDecimal == 50)
            {
                return RedirectToAction("Index", "EndScreen", new { tabletDeviceNumber });
            }
            else
            {
                rankingList.FinalRankingList = true;
                ViewData["Header"] = $"Table {tabletDeviceStatus.Location} - Round {tabletDeviceStatus.RoundNumber}";
                ViewData["Title"] = $"Final Ranking List - {tabletDeviceStatus.Location}";
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                ViewData["TimerSeconds"] = -1;
                if (AppData.IsIndividual)
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

        public JsonResult PollRanking(int tabletDeviceNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            RankingList rankingList = new RankingList(tabletDeviceNumber);
            return Json(rankingList, JsonRequestBehavior.AllowGet);
        }
    }
}