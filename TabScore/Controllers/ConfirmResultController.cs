// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2020 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

using System.Web.Mvc;
using TabScore.Models;

namespace TabScore.Controllers
{
    public class ConfirmResultController : Controller
    {
        public ActionResult Index()
        {
            ViewData["BackButton"] = "TRUE";
            return View(Session["Result"] as Result);
        }

        public ActionResult OKButtonClick()
        {
            (Session["Result"] as Result).UpdateDB();
            return RedirectToAction("Index", "ShowTraveller");
        }

        public ActionResult BackButtonClick()
        {
            Result result = Session["Result"] as Result;
            if (result.ContractLevel == 0)  // This was passed out, so Back goes all the way to Enter Contract screen
            {
                return RedirectToAction("Index", "EnterContract", new { boardNumber = result.BoardNumber });
            }
            else
            {
                return RedirectToAction("Index", "EnterTricksTaken");
            }
        }
    }
}
