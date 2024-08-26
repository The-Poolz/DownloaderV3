using DownloaderContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DownloaderV2.Helpers;

public class SqlQueryHelper(DbContext context)
{
    public void UpdateDownloaderSettings(DownloaderSettings settings, long endingBlock, long latestBlock)
    {
        lock (context)
        {
            var item = context.Set<DownloaderSettings>().FirstOrDefault(T =>
                T.ChainId == settings.ChainId &&
                T.ResponseType == settings.ResponseType &&
                T.EventHash == settings.EventHash &&
                T.ContractAddress == settings.ContractAddress) ?? ApplicationLogger.LogAndThrowDynamic(new NullReferenceException(nameof(settings))); 
            
            item.StartingBlock = endingBlock;
            item.EndingBlock = latestBlock;

            context.Set<DownloaderSettings>().Update(item);
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
        catch (DbUpdateException ex)
        {
            LogPendingChanges();
            ApplicationLogger.LogAndThrow(ex);
        }
    }

    private void LogPendingChanges()
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");

            foreach (var property in entry.CurrentValues.Properties)
            {
                var propName = property.Name;
                var propValue = entry.CurrentValues[propName];
                ApplicationLogger.Log(($"Property: {propName}, Value: {propValue}"));
            }
        }
    }
}