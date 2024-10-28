using DownloaderV3.DataBase;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestableDbContext.Mock;

namespace DownloaderV3.Dispatcher.Models;

// TODO: Delete all this folder Models after update DownloaderV3.DataBase version
public class LocalContextWrapper(DownloaderV3Context context)
{
    public Mock<DbSet<DispatcherSettings>> DispatchSettings { get; set; } = new List<DispatcherSettings>().AsQueryable().BuildMockDbSet();

    public DownloaderV3Context Context => context;
}