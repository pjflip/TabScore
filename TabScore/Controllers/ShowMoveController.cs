using TabScore.Models;
using System;
using System.Web.Mvc;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index()
        {
            int x = Convert.ToInt32(Session["Round"]);
            x++;
            Session["Round"] = x.ToString();

            MoveClass m = new MoveClass();
            if (Session["PairNS"].ToString() != "0")
            {
                m = Move.GetMoveInfo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Round"].ToString(), Session["PairNS"].ToString(), "NS");
                if (m.Table == "0")
                {
                    // No move possible, so session complete
                    return RedirectToAction("Index", "EndScreen");
                }
                ViewData["NSNewTable"] = m.Table;
                ViewData["NSNewDirection"] = m.Direction;
                if (m.Table == Session["Table"].ToString() && m.Direction == "NS")
                {
                    ViewData["StayMoveNS"] = "Stay";
                }
                else
                {
                    ViewData["StayMoveNS"] = "Move";
                }
            }

            if (Session["PairEW"].ToString() != "0")
            {
                m = Move.GetMoveInfo(Session["DBConnectionString"].ToString(), Session["SectionID"].ToString(), Session["Round"].ToString(), Session["PairEW"].ToString(), "EW");
                if (m.Table == "0")
                {
                    // No move possible, so session complete
                    return RedirectToAction("Index", "EndScreen");
                }
                ViewData["EWNewTable"] = m.Table;
                ViewData["EWNewDirection"] = m.Direction;
                if (m.Table == Session["Table"].ToString() && m.Direction == "EW")
                {
                    ViewData["StayMoveEW"] = "Stay";
                }
                else
                {
                    ViewData["StayMoveEW"] = "Move";
                }
            }

            ViewData["Round"] = x.ToString();
            ViewData["PairNS"] = Session["PairNS"];
            ViewData["PairEW"] = Session["PairEW"];
            ViewBag.Header = $"Table {Session["SectionLetter"]}{Session["Table"]}";
            ViewData["CancelButton"] = "FALSE";

            if (Session["PairNS"].ToString() == "0" || Session["PairNS"].ToString() == Session["MissingPair"].ToString())
            {
                return View("NSMissing");
            }
            if (Session["PairEW"].ToString() == "0" || Session["PairEW"].ToString() == Session["MissingPair"].ToString())
            {
                return View("EWMissing");
            }
            return View();
        }


        public ActionResult OKButtonClick()
        {
            return RedirectToAction("Index", "ShowRoundInfo");
        }

        public ActionResult ControlButtonClick()
        {
            Session["ControlReturnScreen"] = "ShowRoundInfo";
            return RedirectToAction("Index", "ControlMenu");
        }
    }
}