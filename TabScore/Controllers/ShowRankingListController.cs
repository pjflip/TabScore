// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class ShowRankingListController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            if (Settings.ShowRanking == 1 && tabletDeviceStatus.RoundNumber > 1)  // Show ranking list only from round 2 onwards
            {
                RankingList rankingList = new RankingList(tabletDeviceNumber);
                    
                // Only show the ranking list if it contains something meaningful
                if (rankingList != null && rankingList.Count > 1 && rankingList[0].ScoreDecimal != 0.0)
                {
                    if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
                    ViewData["Title"] = $"{Strings.ShowRankingList} - {tabletDeviceStatus.Location}";
                    ViewData["Header"] = $"{tabletDeviceStatus.Location}: {Strings.Round} {tabletDeviceStatus.RoundNumber}";
                    if (tabletDeviceStatus.AtSitoutTable)
                    {
                        // Can't go back to ShowBoards if it's a sitout and there are no boards to play, so no 'Back' button
                        ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                    }
                    else
                    {
                        ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
                    }

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
            return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = tabletDeviceStatus.RoundNumber + 1 });
        }

        public ActionResult Final(int tabletDeviceNumber)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            RankingList rankingList = new RankingList(tabletDeviceNumber);
            rankingList.FinalRankingList = true;
            ViewData["Header"] = $"{tabletDeviceStatus.Location} - {Strings.Round} {tabletDeviceStatus.RoundNumber}";
            ViewData["Title"] = $"{Strings.ShowFinalRankingList} - {tabletDeviceStatus.Location}";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
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

        public JsonResult PollRanking(int tabletDeviceNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            RankingList rankingList = new RankingList(tabletDeviceNumber);
            return Json(rankingList, JsonRequestBehavior.AllowGet);
        }
    }
}