// TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterHandRecordController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int boardNumber)
        {
            if (!Settings.ManualHandRecordEntry)
            {
                return RedirectToAction("Index", "ShowTraveller", new { tabletDeviceNumber, boardNumber });
            }
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);

            if (HandRecords.HandRecordsList.Find(x => x.SectionID == tableStatus.SectionID && x.BoardNumber == boardNumber) != null)
            {
                // Hand record already exists, so no need to enter it
                return RedirectToAction("Index", "ShowTraveller", new { tabletDeviceNumber, boardNumber });
            }
            EnterHandRecord enterHandRecord = new EnterHandRecord
            {
                SectionID = tableStatus.SectionID,
                BoardNumber = boardNumber,
                TabletDeviceNumber = tabletDeviceNumber
            };

            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
            ViewData["Title"] = $"{Strings.EnterHandRecord}";
            ViewData["Header"] = Utilities.HeaderString(tabletDeviceStatus, tableStatus, boardNumber);
            ViewData["ButtonOptions"] = ButtonOptions.OKDisabledAndBack;
            return View(enterHandRecord);
        }

        public ActionResult OKButtonClick(int tabletDeviceNumber, string NS, string NH, string ND, string NC, string SS, string SH, string SD, string SC, string ES, string EH, string ED, string EC, string WS, string WH, string WD, string WC)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            int boardNumber = tableStatus.ResultData.BoardNumber;
            HandRecord handRecord = new HandRecord
            {
                SectionID = tableStatus.SectionID,
                BoardNumber = boardNumber,
                NorthSpadesPBN = NS,
                NorthHeartsPBN = NH,
                NorthDiamondsPBN = ND,
                NorthClubsPBN = NC,
                SouthSpadesPBN = SS,
                SouthHeartsPBN = SH,
                SouthDiamondsPBN = SD,
                SouthClubsPBN = SC,
                EastSpadesPBN = ES,
                EastHeartsPBN = EH,
                EastDiamondsPBN = ED,
                EastClubsPBN = EC,
                WestSpadesPBN = WS,
                WestHeartsPBN = WH,
                WestDiamondsPBN = WD,
                WestClubsPBN = WC
            };
            handRecord.UpdateDisplay();
            if (Settings.DoubleDummy) handRecord.UpdateDoubleDummy();
            HandRecords.HandRecordsList.Add(handRecord);
            handRecord.UpdateDB();
            return RedirectToAction("Index", "ShowTraveller", new { tabletDeviceNumber, boardNumber });
        }
    }
}
