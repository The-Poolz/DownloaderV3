﻿using System.Reflection;
using Newtonsoft.Json.Linq;
using DownloaderV3.Destination.Models;

namespace DownloaderV3.Source.CovalentDocument.Decoders;

public class DataDecoderFactory
{
    public readonly DataDecoder Decoder;
    private static readonly Type[] DataDecoderTypes = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && t.BaseType == typeof(DataDecoder))
        .ToArray();

    public DataDecoderFactory(DownloaderMapping map, JToken source)
    {
        var config = new DecoderConfiguration(map);
        Decoder = GetConverter(config.Parameters.Decoder);
        Decoder.Initialize(config, source);
    }

    public static DataDecoder GetConverter(string converterName)
    {
        var converterType = DataDecoderTypes.FirstOrDefault(x => x.Name == converterName) ??
            throw new InvalidOperationException($"Data decoder with name '{converterName}' not found.");
        return (DataDecoder)Activator.CreateInstance(converterType)!;
    }
}