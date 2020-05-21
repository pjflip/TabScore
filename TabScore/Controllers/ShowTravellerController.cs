// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index(int boardNumber)
        {
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "ShowBoards");
            }

            Result result = Session["Result"] as Result;
            if (result == null || result.BoardNumber != boardNumber)
            {
                // No session result data for this board, so must have come from ShowBoards screen View button
                ViewData["BackButton"] = "FALSE";
            }
            else
            {
                ViewData["BackButton"] = "TRUE";
            }

            Traveller traveller = new Traveller((Session["SessionData"] as SessionData).SectionID, boardNumber, (Session["Round"] as Round).PairNS);
            if (AppData.IsIndividual)
            {
                return View("Individual", traveller);
            }
            else
            {
                return View("Pairs", traveller);
            }
        }
    }
}
