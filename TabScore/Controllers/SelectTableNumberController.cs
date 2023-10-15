// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;
using Resources;
using System.Web;
using System;

namespace TabScore.Controllers
{
    public class SelectTableNumberController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber = 0, bool confirm = false) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            SelectTableNumber selectTableNumber = new SelectTableNumber(section, tableNumber, confirm);
            ViewData["Title"] = $"{Strings.SelectTableNumber} - {Strings.Section} {section.SectionLetter}";
            ViewData["Header"] = $"{Strings.Section} {section.SectionLetter}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            return View(selectTableNumber);   
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, bool confirm)
        {
            // Register table in database
            Utilities.RegisterTable(sectionID, tableNumber);
            
            // Check if table status has already been created; if not, add it to the list
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);
            if (tableStatus == null)
            {
                tableStatus = new TableStatus(sectionID, tableNumber, Utilities.GetLastRoundWithResults(sectionID));
                AppData.TableStatusList.Add(tableStatus);
            }
            
            if (AppData.SectionsList.Find(x => x.SectionID == sectionID).TabletDevicesPerTable == 1)
            {
                // One tablet device per table
                TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);

                // Check if tablet device is already registered for this location
                if (tabletDeviceStatus != null && confirm)
                {
                    // Ok to change to this tablet, so set cookie
                    SetCookie(sectionID, tableNumber);
                }
                else if (tabletDeviceStatus == null)
                {
                    // Not on list, so need to add it.  Direction is fixed as North as only one tablet per table
                    tabletDeviceStatus = new TabletDeviceStatus(sectionID, tableNumber, Direction.North, tableStatus.RoundData.NumberNorth, tableStatus.RoundNumber);
                    AppData.TabletDeviceStatusList.Add(tabletDeviceStatus);
                    SetCookie(sectionID, tableNumber);
                }
                else
                {
                    // Check if table number cookie has not been set - if so go back to confirm
                    if (!CheckCookie(sectionID, tableNumber)) 
                    {                    
                        return RedirectToAction("Index", "SelectTableNumber", new { sectionID, tableNumber, confirm = true });
                    }
                    // else = Cookie is Ok, so this is a re-registration and nothing more to do
                }

                // tabletDeviceNumber is the key for identifying this particular tablet device and is used throughout the rest of the application
                int tabletDeviceNumber = AppData.TabletDeviceStatusList.LastIndexOf(tabletDeviceStatus);

                if (tableStatus.ReadyForNextRoundNorth)
                {
                    return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = tableStatus.RoundNumber + 1 });
                }
                else if (tabletDeviceStatus.RoundNumber == 1 || Settings.NumberEntryEachRound)
                {
                    return RedirectToAction("Index", "ShowPlayerIDs", new { tabletDeviceNumber });
                }
                else
                {
                    return RedirectToAction("Index", "ShowRoundInfo", new { tabletDeviceNumber });
                } 
            }
            else   // More than one tablet device per table, so need to know direction for this tablet device
            {
                return RedirectToAction("Index", "SelectDirection", new { sectionID, tableNumber, tableStatus.RoundNumber });
            }
        }

        // Set a cookie for this device
        private void SetCookie(int sectionID, int tableNumber)
        {
            HttpCookie tabScoreCookie = new HttpCookie("tabScore");
            tabScoreCookie.Values["sectionID"] = sectionID.ToString();
            tabScoreCookie.Values["tableNumber"] = tableNumber.ToString();
            Response.Cookies.Add(tabScoreCookie);
        }

        // Check if matching cookie set
        private bool CheckCookie(int sectionID, int tableNumber)
        {
            HttpCookie tabScoreCookie = Request.Cookies["tabScore"];
            if (tabScoreCookie == null) return false;
            int cookieSectionID = Convert.ToInt32(tabScoreCookie.Values["sectionID"]);
            int cookieTableNumber = Convert.ToInt32(tabScoreCookie.Values["tableNumber"]);
            if (cookieSectionID == sectionID && cookieTableNumber == tableNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}