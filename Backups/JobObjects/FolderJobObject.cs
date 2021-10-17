using System;
using System.IO;
using System.Linq;
using Backups.Factories;
using Backups.Models;
using Backups.Packers;
using Backups.PackingAlgorithm;
using Backups.Repositories;
using Backups.Tools;
using Microsoft.Extensions.DependencyInjection;
using Utility.Extensions;

namespace Backups.JobObjects
{
    public class FolderJobObject : IJobObject
    {
        private readonly Repository _repository;
        private readonly IPacker _packer;
        private readonly JobObjectFactory _factory;
        private readonly string _fullPath;
        private readonly ILogger? _logger;

        private readonly IServiceProvider _provider;

        internal FolderJobObject(
            string fullPath,
            Repository repository,
            JobObjectFactory factory,
            IServiceProvider provider)
        {
            _fullPath = fullPath.ThrowIfNull(nameof(fullPath));
            _repository = repository.ThrowIfNull(nameof(repository));
            _packer = provider.GetRequiredService<IPackingAlgorithm>().Packer;
            _factory = factory.ThrowIfNull(nameof(factory));
            Name = Path.GetFileName(fullPath);
            _logger = provider.GetService<ILogger>();
            _provider = provider;

            _logger?.OnComment($"{nameof(FolderJobObject)} has been created at {nameof(Repository)}: {repository}");
        }

        public string Name { get; }
        public int Version { get; private set; }

        public Package GetPackage()
        {
            _logger?.OnMessage($"{nameof(FolderJobObject)}: {this} getting it's contents streams from Repository: {_repository}");
            Package[] packages = _repository
                .GetContentsOf(_fullPath)
                .Select(p => _factory.AtPath(p))
                .Select(o => o.Create(_provider))
                .Select(o => o.GetPackage())
                .ToArray();

            _logger?.OnMessage($"{nameof(FolderJobObject)}: {this} packing it's contents streams");
            Package result = _packer.Pack(ToString(), _logger, packages);

            foreach (Package data in packages)
            {
                data.Stream.Dispose();
            }

            _logger?.OnComment($"{nameof(FolderJobObject)}: {this} content's streams are disposed");

            return result;
        }

        public void IncrementVersion()
        {
            _logger?.OnComment($"{nameof(FolderJobObject)}'s: {this} version is about to be incremented");
            Version++;
        }

        public bool Equals(IJobObject? other)
            => other is FolderJobObject folderJobObject &&
               folderJobObject._repository.Equals(_repository) &&
               folderJobObject._fullPath.Equals(_fullPath);

        public override string ToString()
            => $"[{Version}]_{Name}";
    }
}