using DownloaderV2.Models.Covalent;
using DownloaderV2.Models.LastBlock;

namespace DownloaderV2.Tests.Results.CovalentResults;

public static class CovalentResult
{
    public static BaseInputData<InputData> SwapBNBParty => new(CovalentResultConst.SwapBNBPartyString);
    public static BaseInputData<LastBlockResponse> LastBlock => new(CovalentResultConst.LastBlockString);
}
