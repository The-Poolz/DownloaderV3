namespace DownloaderV2.Models.Api;

public interface IDecoded
{
    string Name { get; set; }
    string Signature { get; set; }
    IParams[] Params { get; set; }
}