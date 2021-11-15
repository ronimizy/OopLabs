using Backups.Repositories;
using Utility.Extensions;

namespace Backups.Models
{
    public class JobObjectConfiguration
    {
        public JobObjectConfiguration(SerializedConfiguration<Repository> repositoryConfiguration, string path)
        {
            RepositoryConfiguration = repositoryConfiguration.ThrowIfNull(nameof(repositoryConfiguration));
            Path = path.ThrowIfNull(nameof(path));
        }

        public SerializedConfiguration<Repository> RepositoryConfiguration { get; }
        public string Path { get; }
    }
}