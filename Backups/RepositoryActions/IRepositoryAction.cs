using Backups.Repositories;
using Backups.Tools;

namespace Backups.RepositoryActions
{
    public interface IRepositoryAction<out T>
    {
        T Execute(Repository repository, ILogger? logger);
    }
}