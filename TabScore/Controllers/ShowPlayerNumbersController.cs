// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowPlayerNumbersController : Controller
    {
        public ActionResult Index()
        {
            Round round = Session["Round"] as Round;
            SessionData sessionData = Session["SessionData"] as SessionData;

            ViewData["BackButton"] = "FALSE";
            Session["Header"] = $"Table {sessionData.SectionTableString} - Round {round.RoundNumber}";

            if (round.PairNS == 0 || round.PairNS == sessionData.MissingPair)
            {
                if (AppData.IsIndividual)
                {
                    return View("NSMissingIndividual", round);
                }
                else
                {
                    return View("NSMissing", round);
                }
            }
            else if (round.PairEW == 0 || round.PairEW == sessionData.MissingPair)
            {
                if (AppData.IsIndividual)
                {
                    return View("EWMissingIndividual", round);
                }
                else
                {
                    return View("EWMissing", round);
                }
            }
            else
            {
                if (AppData.IsIndividual)
                {
                    return View("Individual", round);
                }
                else
                {
                   return View("Pair", round);
                }
            }
        }

        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRoundInfo");
        }
    }
}