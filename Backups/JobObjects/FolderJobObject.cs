using System;
using System.IO;
using System.Linq;
using Backups.Models;
using Backups.Packers;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.JobObjects
{
    public class FolderJobObject : IJobObject
    {
        private readonly IPacker _packer;
        private readonly ILogger? _logger;

        private readonly BackupJobConfiguration _jobConfiguration;

        internal FolderJobObject(
            string fullPath,
            Repository repository,
            BackupJobConfiguration jobConfiguration)
        {
            _jobConfiguration = jobConfiguration.ThrowIfNull(nameof(jobConfiguration));

            Name = Path.GetFileName(fullPath);
            FullPath = fullPath.ThrowIfNull(nameof(fullPath));
            Repository = repository.ThrowIfNull(nameof(repository));
            _packer = jobConfiguration.Packer;
            _logger = jobConfiguration.Logger;

            _logger?.OnComment($"{nameof(FolderJobObject)} has been created at {nameof(Repositories.Repository)}: {repository}");
        }

        public string Name { get; }
        public string FullPath { get; }
        public JobObjectConfiguration Configuration => new JobObjectConfiguration(new SerializedConfiguration<Repository>(Repository), FullPath);
        public Repository Repository { get; }

        public Stream GetStream()
        {
            _logger?.OnMessage($"{nameof(FolderJobObject)}: {this} getting it's contents streams from Repository: {Repository}");
            Package[] packages = Repository
                .GetContentsOf(FullPath)
                .Select(p => Repository.GetObject.AtPath(p))
                .Select(o => o.Build(_jobConfiguration))
                .Select(o => new Package(o.Name, o.GetStream()))
                .ToArray();

            _logger?.OnMessage($"{nameof(FolderJobObject)}: {this} packing it's contents streams");
            Package result = _packer.Pack(Name, _logger, packages);

            foreach (Package data in packages)
            {
                data.Dispose();
            }

            _logger?.OnComment($"{nameof(FolderJobObject)}: {this} content's streams are disposed");

            return result.Stream;
        }

        public bool Equals(IJobObject? other)
            => other is FolderJobObject folderJobObject &&
               folderJobObject.Repository.Equals(Repository) &&
               folderJobObject.FullPath.Equals(FullPath);

        public override bool Equals(object? obj)
            => Equals(obj as IJobObject);

        public override int GetHashCode()
            => HashCode.Combine(Repository, FullPath);

        public override string ToString()
            => Name;
    }
}