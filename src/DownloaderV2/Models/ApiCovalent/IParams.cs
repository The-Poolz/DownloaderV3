namespace DownloaderV2.Models.ApiCovalent;

public interface IParams
{
    string Name { get; set; }
    string Type { get; set; }
    bool Indexed { get; set; }
    bool Decoded { get; set; }
    string Value { get; set; }
}