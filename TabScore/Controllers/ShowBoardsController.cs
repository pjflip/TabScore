// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ShowBoardsController : Controller
    {
       public ActionResult Index()
        {
            // Reset default session value as no board has yet been selected
            Session["Result"] = null;

            Round round = Session["Round"] as Round;
            SessionData sessionData = Session["SessionData"] as SessionData;

            ResultsList resultsList = new ResultsList(sessionData, round);

            if (AppData.IsIndividual)
            {
                Session["Header"] = $"Table {sessionData.SectionTableString} - Round {round.RoundNumber} - {round.PairNS}+{round.South} v {round.PairEW}+{round.West}";
            }
            else
            {
                Session["Header"] = $"Table {sessionData.SectionTableString} - Round {round.RoundNumber} - NS {round.PairNS} v EW {round.PairEW}";
            }
            ViewData["BackButton"] = "FALSE";

            return View(resultsList);
        }
    }
}