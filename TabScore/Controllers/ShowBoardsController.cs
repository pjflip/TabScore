// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index(int sectionID, int tableNumber)
        {
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            ResultsList resultsList = new ResultsList(tableStatus);
            tableStatus.ResultData = null;  // No board selected yet

            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - {tableStatus.RoundData.PairNS}+{tableStatus.RoundData.South} v {tableStatus.RoundData.PairEW}+{tableStatus.RoundData.West}";
            }
            else
            {
                ViewData["Header"] = $"Table {tableStatus.SectionTableString} - Round {tableStatus.RoundData.RoundNumber} - NS {tableStatus.RoundData.PairNS} v EW {tableStatus.RoundData.PairEW}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Show Boards - {tableStatus.SectionTableString}";

            return View(resultsList);
        }
    }
}