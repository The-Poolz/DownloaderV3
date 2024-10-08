﻿using Nethereum.Hex.HexTypes;

namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class HexToLong : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = long.Parse(new HexUTF8String(topicData).Value, System.Globalization.NumberStyles.HexNumber);
    }
}