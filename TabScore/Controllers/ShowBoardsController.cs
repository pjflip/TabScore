// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
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
            
            if (Settings.ShowTimer)
            {
                DateTime startTime;
                int secondsPerRound;
                RoundTimer roundTimer = AppData.RoundTimerList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.RoundNumber == tabletDeviceStatus.RoundNumber);
                if (roundTimer == null)  // Round not yet started, so create timer data for this section and round 
                {
                    startTime = DateTime.Now;
                    secondsPerRound = Convert.ToInt32((resultsList.Count * Settings.MinutesPerBoard + Settings.AdditionalMinutesPerRound) * 60);
                    AppData.RoundTimerList.Add(new RoundTimer
                    {
                        SectionID = tabletDeviceStatus.SectionID,
                        RoundNumber = tabletDeviceStatus.RoundNumber,
                        StartTime = startTime,
                        SecondsPerRound = secondsPerRound
                    });
                }
                else
                {
                    startTime = roundTimer.StartTime;
                    secondsPerRound = roundTimer.SecondsPerRound;
                }
                // Calculate how many seconds remaining for the round
                int timerSeconds = secondsPerRound - Convert.ToInt32(DateTime.Now.Subtract(startTime).TotalSeconds);
                if (timerSeconds < 0) timerSeconds = 0;
                ViewData["TimerSeconds"] = timerSeconds;
            }
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