// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index(int tabletDeviceNumber)
       {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            BoardsList boardsList = new BoardsList(tabletDeviceNumber, tableStatus);
            tableStatus.ResultData = null;  // No board selected yet
            
            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
            ViewData["Title"] = $"{Strings.ShowBoards} - {tabletDeviceStatus.Location}";
            ViewData["Header"] = Utilities.HeaderString(tabletDeviceStatus, tableStatus, -1);
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;

            if (tabletDeviceStatus.Direction == Direction.North)
            {
                return View("Scoring", boardsList);
            }
            else
            {
                boardsList.Message = "NOMESSAGE";
                return View("ViewOnly", boardsList);
            }
       }

        public ActionResult ViewResult(int tabletDeviceNumber, int boardNumber)
        {
            // Only used by ViewOnly view, for tablet device that is not being used for scoring, to check if result has been entered for this board
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            BoardsList boardsList = new BoardsList(tabletDeviceNumber, tableStatus);
            Result result = boardsList.Find(x => x.BoardNumber == boardNumber);
            if (result == null || result.ContractLevel < 0)
            {
                boardsList.Message = "NORESULT";
                if (AppData.IsIndividual)
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth} v {tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}";
                }
                else
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {Strings.N}{Strings.S} {tableStatus.RoundData.NumberNorth} v {Strings.E}{Strings.W} {tableStatus.RoundData.NumberEast}";
                }
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                ViewData["Title"] = $"{Strings.ShowBoards} - {tabletDeviceStatus.Location}";
                ViewData["TabletDeviceNumber"] = tabletDeviceNumber;
                return View("ViewOnly", boardsList);
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
            BoardsList boardsList = new BoardsList(tabletDeviceNumber, tableStatus);
            if (!boardsList.GotAllResults)
            {
                boardsList.Message = "NOTALLRESULTS";
                if (AppData.IsIndividual)
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {tableStatus.RoundData.NumberNorth}+{tableStatus.RoundData.NumberSouth} v {tableStatus.RoundData.NumberEast}+{tableStatus.RoundData.NumberWest}";
                }
                else
                {
                    ViewData["Header"] = $"{tabletDeviceStatus.Location} - {Strings.Round} {tableStatus.RoundNumber} - {Strings.N}{Strings.S} {tableStatus.RoundData.NumberNorth} v {Strings.E}{Strings.W} {tableStatus.RoundData.NumberEast}";
                }
                ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
                ViewData["Title"] = $"{Strings.ShowBoards}  - {tabletDeviceStatus.Location}";
                ViewData["TabletDeviceNumber"] = tabletDeviceNumber;
                return View("ViewOnly", boardsList);
            }
            else
            {
                return RedirectToAction("Index", "ShowRankingList", new { tabletDeviceNumber });
            }
        }
    }
}