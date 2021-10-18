using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.RepositoryActions
{
    [Serializable]
    public class SendPackagesAction : IRepositoryAction<object?>
    {
        [JsonConstructor]
        public SendPackagesAction(string path, IReadOnlyCollection<Package> packages)
        {
            Path = path.ThrowIfNull(nameof(path));
            Packages = packages.ThrowIfNull(nameof(packages));
        }

        public string Path { get; }
        public IReadOnlyCollection<Package> Packages { get; }

        public object? Execute(Repository repository, ILogger? logger)
        {
            repository.ThrowIfNull(nameof(repository));

            foreach (Package package in Packages)
            {
                logger?.OnComment($"{nameof(SendPackagesAction)} started witting of Package: {package} to Repository: {repository}");
                repository.Write($"{Path}{System.IO.Path.DirectorySeparatorChar}{package.Name}", package.Stream);
            }

            return null;
        }
    }
}