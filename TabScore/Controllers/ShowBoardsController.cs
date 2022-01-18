// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index(int tabletDeviceNumber)
       {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            ResultsList resultsList = new ResultsList(tabletDeviceNumber, tableStatus);
            tableStatus.ResultData = null;  // No board selected yet

            if (AppData.IsIndividual)
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth} v {tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}";
            }
            else
            {
                ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - NS {tableStatus.RoundData.NumberNorth} v EW {tableStatus.RoundData.NumberEast}";
            }
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Show Boards - {tabletDeviceStatus.Location}";

            if (tabletDeviceStatus.Direction == Direction.North)
            {
                return View("Scoring", resultsList);
            }
            else
            {
                resultsList.Message = "NOMESSAGE";
                return View("ViewOnly", resultsList);
            }
       }

        public ActionResult ViewResult(int tabletDeviceNumber, int boardNumber)
        {
            // Only used by ViewOnly view, for tablet device that is not being used for scoring, to check if result has been entered for this board
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            ResultsList resultsList = new ResultsList(tabletDeviceNumber, tableStatus);
            Result result = resultsList.Find(x => x.BoardNumber == boardNumber);
            if (result == null || result.ContractLevel < 0)
            {
                resultsList.Message = "NORESULT";
                if (AppData.IsIndividual)
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth} v {tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}";
                }
                else
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - NS {tableStatus.RoundData.NumberNorth} v EW {tableStatus.RoundData.NumberEast}";
                }
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                ViewData["Title"] = $"Show Boards - {tabletDeviceStatus.Location}";
                ViewData["TabletDeviceNumber"] = tabletDeviceNumber;
                return View("ViewOnly", resultsList);
            }
            else
            {
                return RedirectToAction("Index", "ShowTraveller", new { tabletDeviceNumber, boardNumber, fromView = true });
            }
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber)
        {
            // Only used by ViewOnly view, for tablet device that is not being used for scoring, to check if all results have been entered
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            ResultsList resultsList = new ResultsList(tabletDeviceNumber, tableStatus);
            if (!resultsList.GotAllResults)
            {
                resultsList.Message = "NOTALLRESULTS";
                if (AppData.IsIndividual)
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - {tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth} v {tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}";
                }
                else
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tableStatus.RoundNumber} - NS {tableStatus.RoundData.NumberNorth} v EW {tableStatus.RoundData.NumberEast}";
                }
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                ViewData["Title"] = $"Show Boards - {tabletDeviceStatus.Location}";
                ViewData["TabletDeviceNumber"] = tabletDeviceNumber;
                return View("ViewOnly", resultsList);
            }
            else
            {
                return RedirectToAction("Index", "ShowRankingList", new { tabletDeviceNumber });
            }
        }
    }
}