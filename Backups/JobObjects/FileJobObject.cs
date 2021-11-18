using System;
using System.IO;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.JobObjects
{
    public class FileJobObject : IJobObject
    {
        private readonly ILogger? _logger;

        internal FileJobObject(
            string fullPath,
            Repository repository,
            BackupJobConfiguration jobConfiguration)
        {
            FullPath = fullPath.ThrowIfNull(nameof(fullPath));
            Repository = repository.ThrowIfNull(nameof(fullPath));
            Name = Path.GetFileName(fullPath);
            _logger = jobConfiguration.Logger;

            _logger?.OnComment($"{nameof(FileJobObject)} has been created at {nameof(Repositories.Repository)}: {repository}");
        }

        public string Name { get; }
        public string FullPath { get; }
        public JobObjectConfiguration Configuration => new JobObjectConfiguration(new SerializedConfiguration<Repository>(Repository), FullPath);
        public Repository Repository { get; }

        public Stream GetStream()
        {
            _logger?.OnMessage($"{nameof(FileJobObject)}: {this} getting the stream from Repository: {Repository}");
            return Repository.GetStream(FullPath);
        }

        public bool Equals(IJobObject? other)
            => other is FileJobObject fileJobObject &&
               fileJobObject.Repository.Equals(Repository) &&
               fileJobObject.FullPath.Equals(FullPath);

        public override bool Equals(object? obj)
            => Equals(obj as IJobObject);

        public override int GetHashCode()
            => HashCode.Combine(Repository, FullPath);

        public override string ToString()
            => Name;
    }
}