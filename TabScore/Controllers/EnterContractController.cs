// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System;
using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class EnterContractController : Controller
    {
        public ActionResult Index(int boardNumber)
        {
            Round round = Session["Round"] as Round;
            Result result = Session["Result"] as Result;
            SessionData sessionData = Session["SessionData"] as SessionData;

            if (result == null || result.BoardNumber != boardNumber)   // No session result data for this board
            {
                result = new Result()
                {
                    SectionID = sessionData.SectionID,
                    TableNumber = sessionData.TableNumber,
                    RoundNumber = round.RoundNumber,
                    BoardNumber = boardNumber,
                    PairNS = round.PairNS,
                    PairEW = round.PairEW
                };
                if (AppData.IsIndividual)
                {
                    result.South = round.South;
                    result.West = round.West;
                }
                result.ReadDB();
                Session["Result"] = result;   // Need to update Session object, as implicit reference is broken
            }

            if (AppData.IsIndividual)
            {
                Session["Header"] = $"Table {sessionData.SectionTableString} - Round {round.RoundNumber} - {UtilityFunctions.ColourPairByVulnerability("NS", boardNumber, $"{round.PairNS}+{round.South}")} v {UtilityFunctions.ColourPairByVulnerability("EW", boardNumber, $"{round.PairEW}+{round.West}")}";
            }
            else
            {
                Session["Header"] = $"Table {sessionData.SectionTableString} - Round {round.RoundNumber} - {UtilityFunctions.ColourPairByVulnerability("NS", boardNumber, $"NS {round.PairNS}")} v {UtilityFunctions.ColourPairByVulnerability("EW", boardNumber, $"EW {round.PairEW}")}";
            }
            ViewData["BackButton"] = "TRUE";
            return View(result);
        }

        public ActionResult OKButtonContract(string cLevel, string cSuit, string cX, string cNSEW)
        {
            Result result = Session["Result"] as Result;
            result.ContractLevel = Convert.ToInt32(cLevel);
            result.ContractSuit = cSuit;
            result.ContractX = cX;
            result.NSEW = cNSEW;
            return RedirectToAction("Index", "EnterLead", new { validateWarning = "Validate" });
        }

        public ActionResult OKButtonPass()
        {
            Result result = Session["Result"] as Result;
            result.ContractLevel = 0;
            result.ContractSuit = "";
            result.ContractX = "";
            result.NSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;
            result.CalculateScore();
            return RedirectToAction("Index", "ConfirmResult");
        }
        
        public ActionResult OKButtonSkip()
        {
            Result result = Session["Result"] as Result;
            result.ContractLevel = -1;
            result.ContractSuit = "";
            result.ContractX = "";
            result.NSEW = "";
            result.LeadCard = "";
            result.TricksTakenNumber = -1;

            result.UpdateDB();
            return RedirectToAction("Index", "ShowBoards");
        }
    }
}
