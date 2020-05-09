// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowTravellerController : Controller
    {
        public ActionResult Index()
        {
            if (!Settings.ShowResults)
            {
                return RedirectToAction("Index", "ShowBoards");
            }

            Result result = Session["Result"] as Result;
            Traveller traveller = new Traveller((Session["SessionData"] as SessionData).SectionID, result.BoardNumber, result.PairNS);
            ViewData["BackButton"] = "TRUE";
            if (AppData.IsIndividual)
            {
                return View("Individual", traveller);
            }
            else
            {
                return View("Pairs", traveller);
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult BackButtonClick()
        {
            return RedirectToAction("Index", "ConfirmResult");
        }

        public ActionResult HandRecordButtonClick()
        {
            return RedirectToAction("Index", "ShowHandRecord");
        }
    }
}
