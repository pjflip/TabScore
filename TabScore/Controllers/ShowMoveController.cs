using TabScore.Models;
using System.Web.Mvc;
using System;

namespace TabScore.Controllers
{
    public class ShowMoveController : Controller
    {
        public ActionResult Index(int newRoundNumber)
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (newRoundNumber > Convert.ToInt32(Session["MaxRounds"]))  // Session complete
            {
                if (Settings.GetSetting<int>(DBConnectionString, SettingName.ShowRanking) == 2)
                {
                    return RedirectToAction("Index", "ShowFinalRankingList");
                }
                else
                {
                    return RedirectToAction("Index", "EndScreen");
                }
            }

            Round round = Session["Round"] as Round;
            MoveView moveView = new MoveView();
            moveView.RoundNumber = newRoundNumber;
            if (Convert.ToBoolean(Session["IndividualEvent"]))
            {
                if (round.PairNS != 0)
                {
                    Move move = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, round.PairNS);
                    moveView.PairNS = round.PairNS;
                    moveView.NorthNewTable = move.Table;
                    moveView.NorthNewDirection = move.Direction;
                    moveView.NorthStay = (move.Table == Convert.ToInt32(Session["Table"]) && move.Direction == "North");
                }
                if (round.PairEW != 0)
                {
                    Move move = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, round.PairEW);
                    moveView.PairEW = round.PairEW;
                    moveView.EastNewTable = move.Table;
                    moveView.EastNewDirection = move.Direction;
                    moveView.EastStay = (move.Table == Convert.ToInt32(Session["Table"]) && move.Direction == "East");
                }
                if (round.South != 0)
                {
                    Move move = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, round.South);
                    moveView.South = round.South;
                    moveView.SouthNewTable = move.Table;
                    moveView.SouthNewDirection = move.Direction;
                    moveView.SouthStay = (move.Table == Convert.ToInt32(Session["Table"]) && move.Direction == "South");
                }
                if (round.West != 0)
                {
                    Move move = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, round.West);
                    moveView.West = round.West;
                    moveView.WestNewTable = move.Table;
                    moveView.WestNewDirection = move.Direction;
                    moveView.WestStay = (move.Table == Convert.ToInt32(Session["Table"]) && move.Direction == "West");
                }
            }
            else
            {
                if (round.PairNS != 0)
                {
                    Move move = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, round.PairNS, "NS");
                    moveView.PairNS = round.PairNS;
                    moveView.NorthNewTable = move.Table;
                    moveView.NorthNewDirection = move.Direction;
                    moveView.NorthStay = (move.Table == Convert.ToInt32(Session["Table"]) && move.Direction == "NS");
                }
                if (round.PairEW != 0)
                {
                    Move move = new Move(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, round.PairEW, "EW");
                    moveView.PairEW = round.PairEW;
                    moveView.EastNewTable = move.Table;
                    moveView.EastNewDirection = move.Direction;
                    moveView.EastStay = (move.Table == Convert.ToInt32(Session["Table"]) && move.Direction == "EW");
                }
            }

            moveView.LowBoard = round.LowBoard;
            moveView.HighBoard = round.HighBoard;
            BoardMove boardMove = new BoardMove(DBConnectionString, Convert.ToInt32(Session["SectionID"]), newRoundNumber, Convert.ToInt32(Session["Table"]), round.LowBoard);
            moveView.BoardsNewTable = boardMove.Table;
            
            Session["Header"] = $"Table {Session["SectionLetter"]}{Session["Table"]}";
            ViewData["BackButton"] = "FALSE";

            moveView.NSMissing = (round.PairNS == 0 || round.PairNS == Convert.ToInt32(Session["MissingPair"]));
            moveView.EWMissing = (round.PairEW == 0 || round.PairEW == Convert.ToInt32(Session["MissingPair"]));

            // Update session to new round info
            Session["Round"] = new Round(DBConnectionString, Convert.ToInt32(Session["SectionID"]), Convert.ToInt32(Session["Table"]), newRoundNumber, Convert.ToBoolean(Session["IndividualEvent"]));

            if (Convert.ToBoolean(Session["IndividualEvent"]))
            {
                return View("Individual", moveView);
            }
            else
            {
                return View("Pair", moveView);
            }
        }

        public ActionResult OKButtonClick()
        {
            string DBConnectionString = Session["DBConnectionString"].ToString();
            if (DBConnectionString == "") return RedirectToAction("Index", "ErrorScreen");

            if (Settings.GetSetting<bool>(DBConnectionString, SettingName.NumberEntryEachRound))
            {
                return RedirectToAction("Index", "ShowPlayerNumbers");
            }
            else
            {
                return RedirectToAction("Index", "ShowRoundInfo");
            }
        }
    }
}