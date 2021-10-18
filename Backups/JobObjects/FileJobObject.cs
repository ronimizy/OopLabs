using System;
using System.IO;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Microsoft.Extensions.DependencyInjection;
using Utility.Extensions;

namespace Backups.JobObjects
{
    public class FileJobObject : IJobObject
    {
        private readonly Repository _repository;
        private readonly string _fullPath;
        private readonly ILogger? _logger;

        internal FileJobObject(
            string fullPath,
            Repository repository,
            IServiceProvider provider)
        {
            _fullPath = fullPath.ThrowIfNull(nameof(fullPath));
            _repository = repository.ThrowIfNull(nameof(fullPath));
            Name = Path.GetFileName(fullPath);
            _logger = provider.GetService<ILogger>();

            _logger?.OnComment($"{nameof(FileJobObject)} has been created at {nameof(Repository)}: {repository}");
        }

        public string Name { get; }
        public int Version { get; private set; }

        public Package GetPackage()
        {
            _logger?.OnMessage($"{nameof(FileJobObject)}: {this} getting the stream from Repository: {_repository}");
            Stream sourceStream = _repository.GetStream(_fullPath);
            return new Package(ToString(), sourceStream);
        }

        public void IncrementVersion()
        {
            _logger?.OnComment($"{nameof(FileJobObject)}'s: {this} version is about to be incremented");
            Version++;
        }

        public bool Equals(IJobObject? other)
            => other is FileJobObject fileJobObject &&
               fileJobObject._repository.Equals(_repository) &&
               fileJobObject._fullPath.Equals(_fullPath);

        public override string ToString()
            => $"[{Version}]_{Name}";
    }
}