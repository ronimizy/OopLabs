using System;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.Storages
{
    public interface IStorage : IDisposable
    {
        void WriteTo(Repository repository, ILogger? logger);
    }
}