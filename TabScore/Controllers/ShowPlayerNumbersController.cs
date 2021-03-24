// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2021 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, bool showWarning = false)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);

            // Player numbers not needed if all names have already been entered and names are not being updated each round, or if this a sitout table
            if ((!tableStatus.RoundData.BlankName && !Settings.NumberEntryEachRound) || tabletDeviceStatus.TableNumber == 0)
            {
                return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
            }

            PlayerEntryList playerEntryList = new PlayerEntryList(tableStatus, tabletDeviceNumber);
            playerEntryList.ShowWarning = showWarning;

            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Show Player Numbers - {tabletDeviceStatus.Location}";
            ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tabletDeviceStatus.RoundNumber}";

            if (AppData.IsIndividual)
            {
                return View("Individual", playerEntryList);
            }
            else
            {
                return View("Pair", playerEntryList);
            }
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber)
        {
            // Check if other tablet devices at the same table have completed player number entry

            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            int missingPair = AppData.SectionsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID).MissingPair;
            Round round = tableStatus.RoundData;
            
            // Check if all required names have been entered, and if not go back and wait
            if (round.NumberNorth == 0 || round.NumberNorth == missingPair)
            {
                if (round.NameEast == "" || round.NameWest == "")
                {
                    return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber, showWarning = true });
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                }
            }
            else if (round.NumberEast == 0 || round.NumberEast == missingPair)
            {
                if (round.NameNorth == "" || round.NameSouth == "")
                {
                    return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber, showWarning = true });
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                }
            }
            else
            {
                if (round.NameNorth == "" || round.NameSouth == "" || round.NameEast == "" || round.NameWest == "")
                {
                    return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber, showWarning = true });
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                }
            }
        }
    }
}