using DownloaderV3.Destination;
using Microsoft.EntityFrameworkCore;
using DownloaderV3.Destination.Models;

namespace DownloaderV3.Helpers;

public class SqlQueryHelper(BaseDestination destination)
{
    public void UpdateDownloaderSettings(DownloaderSettings settings, long endingBlock, long latestBlock)
    {
        lock (destination)
        {
            var item = destination.DownloaderSettings.FirstOrDefault(T =>
                T.ChainId == settings.ChainId &&
                T.ResponseType == settings.ResponseType &&
                T.EventHash == settings.EventHash &&
                T.ContractAddress == settings.ContractAddress) ?? ApplicationLogger.LogAndThrowDynamic(new NullReferenceException(nameof(settings))); 
            
            item.StartingBlock = endingBlock;
            item.EndingBlock = latestBlock;

            destination.DownloaderSettings.Update(item);
        }
    }

    public virtual void TrySaveChangeAsync()
    {
        try
        {
            lock (destination)
            {
                destination.SaveChanges();
            }
        }
        catch (DbUpdateException ex)
        {
            ApplicationLogger.LogAndThrow(ex);
        }
    }
}