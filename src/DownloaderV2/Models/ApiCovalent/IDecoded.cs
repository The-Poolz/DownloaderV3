namespace DownloaderV2.Models.ApiCovalent;

public interface IDecoded
{
    string Name { get; set; }
    string Signature { get; set; }
    IParams[] Params { get; set; }
}