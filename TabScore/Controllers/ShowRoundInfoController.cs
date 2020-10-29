// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus.RoundData.RoundNumber == 1)
            {
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            }
            else 
            {
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabledAndBack;
            }

            ViewData["Title"] = $"Show Round Info - {tableStatus.SectionTableString}";
            ViewData["Header"] = $"Table {tableStatus.SectionTableString}";

            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            if (tableStatus.RoundData.PairNS == 0 || tableStatus.RoundData.PairNS == section.MissingPair)
            {
                if (AppData.IsIndividual)
                {
                   return View("NSMissingIndividual", tableStatus);
                }
                else
                {
                    return View("NSMissing", tableStatus);
                }
            }
            else if (tableStatus.RoundData.PairEW == 0 || tableStatus.RoundData.PairEW == section.MissingPair)
            {
                if (AppData.IsIndividual)
                {
                   return View("EWMissingIndividual", tableStatus);
                }
                else
                {
                    return View("EWMissing", tableStatus);
                }
            }
            else
            {
                if (AppData.IsIndividual)
                {
                    return View("Individual", tableStatus);
                }
                else
                {
                   return View("Pair", tableStatus);
                }
            }
        }

        public ActionResult BackButtonClick(int sectionID, int tableNumber)
        {
            // Reset to the previous round; RoundNumber > 1 else no Back button and cannot get here
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            int roundNumber = tableStatus.RoundData.RoundNumber;
            tableStatus.RoundData =  new Round(sectionID, tableNumber, roundNumber - 1);
            return RedirectToAction("Index", "ShowMove", new { sectionID, tableNumber, newRoundNumber = roundNumber });
        }
    }
}