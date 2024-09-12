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
            LogPendingChanges();
            ApplicationLogger.LogAndThrow(ex);
        }
    }

    private void LogPendingChanges()
    {
        foreach (var entry in destination.ChangeTracker.Entries())
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