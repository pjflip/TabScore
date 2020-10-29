// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EndScreenController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            ViewData["Header"] = $"Table {tableStatus.SectionTableString}";
            ViewData["Title"] = $"Show Move - {tableStatus.SectionTableString}";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            return View(tableStatus);
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber)
        {
            // Check if new round has been added; can't apply to individuals
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.RoundData.RoundNumber == Utilities.NumberOfRoundsInEvent(sectionID))  
            {
                // Final round, so no new rounds added
                return RedirectToAction("Index", "EndScreen", new { sectionID, tableNumber });
            }
            else
            {
                return RedirectToAction("Index", "ShowMove", new { sectionID, tableNumber, newRoundNumber = tableStatus.RoundData.RoundNumber + 1 });
            }
        }
    }
}