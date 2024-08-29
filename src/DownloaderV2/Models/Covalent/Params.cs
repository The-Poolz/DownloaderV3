namespace DownloaderV2.Models.Covalent;

public class Params
{
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool Indexed { get; set; }
    public bool Decoded { get; set; }
    public string Value { get; set; } = null!;
}