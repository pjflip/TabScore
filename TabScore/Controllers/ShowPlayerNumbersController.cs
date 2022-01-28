// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
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

            if (tabletDeviceStatus.NamesUpdateRequired) {
                tableStatus.RoundData.UpdateNames(tableStatus);  // Update names from database if not done very recently
            }

            if (tableStatus.RoundData.GotAllNames && tabletDeviceStatus.RoundNumber > 1 && !Settings.NumberEntryEachRound)
            {
                // Player numbers not needed if all names have already been entered and names are not being updated each round
                tabletDeviceStatus.NamesUpdateRequired = false;  // No round update required in RoundInfo as it's just been done
                return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
            }
            tabletDeviceStatus.NamesUpdateRequired = true;  // We'll now need to update when we get to RoundInfo in case names change in the mean time

            PlayerEntryList playerEntryList = new PlayerEntryList(tabletDeviceNumber, tableStatus);
            playerEntryList.ShowWarning = showWarning;

            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Title"] = $"Show Player Numbers - {tabletDeviceStatus.Location}";
            ViewData["Header"] = $"{tabletDeviceStatus.Location} - Round {tabletDeviceStatus.RoundNumber}";

            ViewData["TimerSeconds"] = -1;
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
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            tableStatus.RoundData.UpdateNames(tableStatus);
            tabletDeviceStatus.NamesUpdateRequired = false;  // No names update required on next screen as it's only just been done

            // Check if all required names have been entered, and if not go back and wait
            if (tableStatus.RoundData.GotAllNames)
            {
                return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
            }
            else
            {
                return RedirectToAction("Index", "ShowPlayerNumbers", new { tabletDeviceNumber, showWarning = true });
            }
        }
    }
}