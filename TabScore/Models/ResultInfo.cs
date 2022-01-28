// TabScore - TabScore, a wireless bridge scoring program.  Copyright(C) 2022 by Peter Flippant
// Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License

namespace TabScore.Models
{
    public class ResultInfo
    {
        public Result ResultData { get; private set; }
        public int TabletDeviceNumber { get; private set; }

        public ResultInfo(int tabletDeviceNumber, TableStatus tableStatus)
        {
            TabletDeviceNumber = tabletDeviceNumber;
            ResultData = tableStatus.ResultData;
        }
    }
}