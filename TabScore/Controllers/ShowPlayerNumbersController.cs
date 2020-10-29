// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Show Player Numbers - {tableStatus.SectionTableString}";
            ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber}";

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
    }
}