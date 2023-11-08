// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;
using Resources;
using System.Web;
using System;

namespace TabScore.Controllers
{
    public class SelectDirectionController : Controller
    {
        public ActionResult Index(int sectionID, int tableNumber, int roundNumber, Direction direction = Direction.Null, bool confirm = false) 
        {
            Section section = AppData.SectionsList.Find(x => x.SectionID == sectionID);
            SelectDirection selectDirection = new SelectDirection(section, tableNumber, direction, roundNumber, confirm);

            ViewData["Title"] = $"{Strings.SelectDirection} - {section.SectionLetter}{tableNumber}";
            ViewData["Header"] = $"{Strings.Table} {section.SectionLetter}{tableNumber}";
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
            if (AppData.IsIndividual)
            {
                return View("Individual", selectDirection);
            }
            else
            {
                return View("Pair", selectDirection);
            }
        }

        public ActionResult OKButtonClick(int sectionID, int tableNumber, Direction direction, int roundNumber, bool confirm)
        {
            // Check if tablet device is already registered for this location, and if so confirm
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber && x.Direction == direction);
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == sectionID && x.TableNumber == tableNumber);

            // Check if tablet device is already registered for this location
            if (tabletDeviceStatus != null && confirm)
            {
                // Ok to change to this tablet, so set cookie
                SetCookie(sectionID, tableNumber, direction);
            }
            else if (tabletDeviceStatus == null)
            {
                // Not on list of registered tablet devices, so need to add it
                int pairNumber = 0;
                if (direction == Direction.North)
                {
                    pairNumber = tableStatus.RoundData.NumberNorth;
                }
                else if (direction == Direction.East)
                {
                    pairNumber = tableStatus.RoundData.NumberEast;
                }
                else if (direction == Direction.South)
                {
                    pairNumber = tableStatus.RoundData.NumberSouth;
                }
                else if (direction == Direction.West)
                {
                    pairNumber = tableStatus.RoundData.NumberWest;
                }
                tabletDeviceStatus = new TabletDeviceStatus(sectionID, tableNumber, direction, pairNumber, roundNumber);
                AppData.TabletDeviceStatusList.Add(tabletDeviceStatus);
                SetCookie(sectionID, tableNumber, direction);
            }
            else
            {
                // Check if table number cookie has not been set - if so go back to confirm
                if (!CheckCookie(sectionID, tableNumber, direction))
                {
                    return RedirectToAction("Index", "SelectDirection", new { sectionID, tableNumber, roundNumber, direction, confirm = true });
                }
                // else = Cookie is Ok, so this is a re-registration and nothing more to do
            }

            // tabletDeviceNumber is the key for identifying this particular tablet device and is used throughout the rest of the application
            int tabletDeviceNumber = AppData.TabletDeviceStatusList.LastIndexOf(tabletDeviceStatus);

            if (((direction == Direction.North) && tableStatus.ReadyForNextRoundNorth) || ((direction == Direction.East) && tableStatus.ReadyForNextRoundEast) || (direction == Direction.South && tableStatus.ReadyForNextRoundSouth) || (direction == Direction.West && tableStatus.ReadyForNextRoundWest))
            {
                return RedirectToAction("Index", "ShowMove", new { tabletDeviceNumber, newRoundNumber = roundNumber + 1 });
            }
            else
            {
                return RedirectToAction("Index", "ShowPlayerIDs", new { tabletDeviceNumber });
            }
        }

        // Set a cookie for this device
        private void SetCookie(int sectionID, int tableNumber, Direction direction)
        {
            HttpCookie tabScoreCookie = new HttpCookie("tabScore");
            tabScoreCookie.Values["sectionID"] = sectionID.ToString();
            tabScoreCookie.Values["tableNumber"] = tableNumber.ToString();
            tabScoreCookie.Values["direction"] = Enum.GetName(typeof(Direction), direction);
            Response.Cookies.Add(tabScoreCookie);
        }

        // Check if matching cookie set
        private bool CheckCookie(int sectionID, int tableNumber, Direction direction)
        {
            HttpCookie tabScoreCookie = Request.Cookies["tabScore"];
            if (tabScoreCookie == null) return false;
            int cookieSectionID = Convert.ToInt32(tabScoreCookie.Values["sectionID"]);
            int cookieTableNumber = Convert.ToInt32(tabScoreCookie.Values["tableNumber"]);
            Direction cookieDirection = (Direction)Enum.Parse(typeof(Direction), tabScoreCookie.Values["direction"]);
            if (cookieSectionID == sectionID && cookieTableNumber == tableNumber && cookieDirection == direction)
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