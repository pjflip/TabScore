// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Reflection;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class StartScreenController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Header"] = "";
            ViewData["Title"] = "Start Screen";
            ViewData["ButtonOptions"] = ButtonOptions.OKEnabled;
            ViewData["Version"] = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ViewData["TimerSeconds"] = -1;
            return View();
        }

        public ActionResult OKButtonClick()
        {
            AppData.Refresh();
            if (AppData.DBConnectionString == "")
            {
                TempData["warningMessage"] = "Scoring database not yet selected";
                return RedirectToAction("Index", "StartScreen");
            }
            Settings.Refresh();
            AppData.SetTabletDevicesPerTable();
            if (Settings.ShowHandRecord || Settings.ValidateLeadCard)
            {
                HandRecords.Refresh();
            }

            return RedirectToAction("Index", "EnterSection");
        }
    }
}