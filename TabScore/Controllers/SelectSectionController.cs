// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2023 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using Resources;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class SelectSectionController : Controller
    {
        public ActionResult Index()
        {
            // Check if only one section - if so use it
            if (AppData.SectionsList.Count == 1)
            {
                return RedirectToAction("Index", "SelectTableNumber", new { sectionID = AppData.SectionsList[0].SectionID });
            }
            else
            // Get section
            {
                ViewData["Title"] = Strings.SelectSection;
                ViewData["Header"] = "";
                ViewData["ButtonOptions"] = ButtonOptions.OKDisabled;
                return View(AppData.SectionsList);
            }
        }
    }
}