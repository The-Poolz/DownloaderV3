using DownloaderContext;
using DownloaderContext.Models;
using DownloaderV2.Helpers.Logger;

namespace DownloaderV2.Helpers;

public class SqlQueryHelper(BaseDownloaderContext context)
{
    public void UpdateDownloaderSettings(DownloaderSettings settings, long endingBlock, long latestBlock)
    {
        lock (context)
        {
            var item = context.DownloaderSettings.FirstOrDefault(T =>
                T.ChainId == settings.ChainId &&
                T.ResponseType == settings.ResponseType &&
                T.EventHash == settings.EventHash &&
                T.ContractAddress == settings.ContractAddress) ?? ApplicationLogger.LogAndThrowDynamic(new NullReferenceException(nameof(settings))); 
            
            item.StartingBlock = endingBlock;
            item.EndingBlock = latestBlock;

            context.DownloaderSettings.Update(item);
        }
    }

    public virtual void TrySaveChangeAsync()
    {
        try
        {
            lock (context)
            {
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            ApplicationLogger.LogAndThrow(ex);
        }
    }
}