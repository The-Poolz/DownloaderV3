namespace DownloaderV2.Models.Covalent;

public class Decoded
{
    public string Name { get; set; } = null!;
    public string Signature { get; set; } = null!;
    public Params[] Params { get; set; } = null!;
}