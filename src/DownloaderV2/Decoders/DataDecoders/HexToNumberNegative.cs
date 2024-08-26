﻿using Nethereum.Util;
using System.Numerics;

namespace DownloaderV2.Decoders.DataDecoders;

public class HexToNumberNegative : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        var bigIntValue = BigInteger.Parse(topicData, System.Globalization.NumberStyles.HexNumber);

        DecodedData = (long)(decimal.Round((decimal)new BigDecimal(bigIntValue), 0));
    }
}