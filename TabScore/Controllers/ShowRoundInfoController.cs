// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowRoundInfoController : Controller
    {
        public ActionResult Index()
        {
            Round round = Session["Round"] as Round;
            if (round.RoundNumber == 1)
            {
                ViewData["BackButton"] = "FALSE";
            }
            else 
            {
                ViewData["BackButton"] = "TRUE";
            }

            SessionData sessionData = Session["SessionData"] as SessionData;
            Session["Header"] = $"Table {sessionData.SectionTableString}";

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
            return RedirectToAction("Index", "ShowBoards");
        }

        public ActionResult OKSitOutButtonClick()
        {
            return RedirectToAction("Index", "ShowRankingList");
        }

        public ActionResult BackButtonClick()
        {
            // Reset to the previous round; RoundNumber > 1 else no Back button and cannot get here
            int roundNumber = (Session["Round"] as Round).RoundNumber;
            SessionData sessionData = Session["SessionData"] as SessionData;
            Session["Round"] = new Round(sessionData, roundNumber - 1);
            return RedirectToAction("Index", "ShowMove", new { newRoundNumber = roundNumber });
        }
    }
}