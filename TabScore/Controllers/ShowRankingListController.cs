// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRankingListController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.RoundData.RoundNumber > 1)  // Show ranking list only from round 2 onwards
            {
                if (Settings.ShowRanking == 1)
                {
                    RankingList rankingList = new RankingList(tableStatus);
                    
                    // Only show the ranking list if it contains something meaningful
                    if (rankingList != null && rankingList.Count != 0 && rankingList[0].ScoreDecimal != 0 && rankingList[0].ScoreDecimal != 50)
                    {
                        ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber}";
                        ViewData["Title"] = $"Ranking List - {tableStatus.SectionTableString}";
                        ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                        if (AppData.IsIndividual)
                        {
                            return View("IndividualRankingList", rankingList);
                        }
                        else if (rankingList.Exists(x => x.Orientation == "E"))
                        {
                            return View("TwoWinnersRankingList", rankingList);
                        }
                        else
                        {
                            return View("OneWinnerRankingList", rankingList);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "ShowMove", new { sectionID, tableNumber, newRoundNumber = tableStatus.RoundData.RoundNumber + 1 });
        }

        public JsonResult PollRanking(int sectionID, int tableNumber)
        {
            HttpContext.Response.AppendHeader("Connection", "close");
            return Json(new RankingList(AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber)), JsonRequestBehavior.AllowGet);
        }
    }
}