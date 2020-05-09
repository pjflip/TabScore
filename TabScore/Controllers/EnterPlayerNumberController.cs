// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterPlayerNumberController : Controller
    {
        public ActionResult Index(string direction)
        {
            ViewData["Direction"] = direction;
            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {(Session["SessionData"] as SessionData).SectionTableString}";
            return View();
        }

        public ActionResult OKButtonClick(string direction, int playerNumber)
        {
            // Update Round with new player
            (Session["Round"] as Round).UpdatePlayer(Session["SessionData"] as SessionData, direction, playerNumber);
            
            return RedirectToAction("Index", "ShowPlayerNumbers");
        }
    }
}