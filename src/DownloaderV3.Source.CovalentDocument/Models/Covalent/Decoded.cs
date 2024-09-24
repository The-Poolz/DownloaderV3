namespace DownloaderV3.Source.CovalentDocument.Models.Covalent;

public class Decoded
{
    public string Name { get; set; } = null!;
    public string Signature { get; set; } = null!;
    public Params[] Params { get; set; } = null!;
}