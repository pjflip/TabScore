using System.Collections.Generic;

namespace TabScore.Models
{
    public class ResultsList : List<Result>
    {
        public bool GotAllResults { get; private set; }

        public ResultsList(SessionData sessionData, Round round)
        {
            int resultCount = 0;
            for (int i = round.LowBoard; i <= round.HighBoard; i++)
            {
                Result result = new Result(sessionData.SectionID, sessionData.TableNumber, round.RoundNumber, i);
                Add(result);
                if (result.ContractLevel != -999) resultCount++;
            }
            GotAllResults = (resultCount == round.HighBoard - round.LowBoard + 1);
        }
    }
}