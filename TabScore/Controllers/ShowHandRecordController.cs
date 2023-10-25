// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;
using Resources;

namespace TabScore.Controllers
{
    public class ShowHandRecordController : Controller
    {
        public ActionResult Index(int tabletDeviceNumber, int boardNumber, bool fromView)
        {
            TabletDeviceStatus tabletDeviceStatus = AppData.TabletDeviceStatusList[tabletDeviceNumber];
            TableStatus tableStatus = AppData.TableStatusList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.TableNumber == tabletDeviceStatus.TableNumber);
            HandRecord handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == tabletDeviceStatus.SectionID && x.BoardNumber == boardNumber);
            if (handRecord == null)     // Can't find matching hand record, so use default SectionID = 1
            {
                handRecord = HandRecords.HandRecordsList.Find(x => x.SectionID == 1 && x.BoardNumber == boardNumber);
            }
            handRecord.TabletDeviceNumber = tabletDeviceNumber;
            handRecord.FromView = fromView;
            handRecord.PerspectiveDirection = tabletDeviceStatus.PerspectiveDirection.ToString();
            handRecord.PerspectiveButtonOption = tabletDeviceStatus.PerspectiveButtonOption;

            if (Settings.ShowTimer) ViewData["TimerSeconds"] = Utilities.SetTimerSeconds(tabletDeviceStatus);
            ViewData["Title"] = $"{Strings.ShowHandRecord} - {tabletDeviceStatus.Location}";
            ViewData["Header"] = Utilities.HeaderString(tabletDeviceStatus, tableStatus, boardNumber);
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            return View(handRecord);
        }
    }
}