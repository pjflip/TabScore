// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int newRoundNumber)
        {
            if (newRoundNumber > Utilities.NumberOfRoundsInEvent(sectionID))  // Session complete
            {
                if (Settings.ShowRanking == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList", new { sectionID, tableNumber });
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen", new { sectionID, tableNumber });
                }
            }

            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            MovesList movesList = new MovesList(tableStatus, newRoundNumber);

            ViewData["Header"] = $"Table {tableStatus.SectionTableString}";
            ViewData["Title"] = $"Show Move - {tableStatus.SectionTableString}";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;

            if (AppData.IsIndividual)
            {
                return View("Individual", movesList);
            }
            else
            {
                return View("Pair", movesList);
            }
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, int newRoundNumber)
        {
            // Refresh settings and hand records for the start of the round
            Settings.Refresh();
            if (Settings.ShowHandRecord || Settings.ValidateLeadCard)
            {
                HandRecords.Refresh();
            }

            // Update table status round info
            AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber).RoundData = new Round(sectionID, tableNumber, newRoundNumber);

            if (Settings.NumberEntryEachRound)
            {
                return RedirectToAction("Index", "ShowPlayerNumbers", new { sectionID, tableNumber });
            }
            else
            {
                return RedirectToAction("Index", "ShowRoundInfo", new { sectionID, tableNumber });
            }
        }
    }
}