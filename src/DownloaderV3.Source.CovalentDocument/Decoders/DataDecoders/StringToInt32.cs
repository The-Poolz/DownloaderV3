﻿namespace DownloaderV3.Source.CovalentDocument.Decoders.DataDecoders;

public class StringToInt32 : DataDecoder
{
    public override void BuildFromData(string topicData)
    {
        DecodedData = Convert.ToInt32(topicData);
    }
}