using Backups.JobObjects;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Factories
{
    public sealed class JobObjectFactory
    {
        private readonly Repository _repository;
        private readonly ILogger? _logger;

        internal JobObjectFactory(Repository repository, ILogger? logger)
        {
            _repository = repository.ThrowIfNull(nameof(repository));
            _logger = logger;
        }

        public JobObjectBuilder AtPath(string path)
        {
            path.ThrowIfNull(nameof(path));

            if (_repository.Exists(path))
            {
                _logger?.OnComment($"JobObjectBuilder for path {path} at repository {_repository} is being created");

                return _repository.IsFolder(path)
                    ? new JobObjectBuilder(c => new FolderJobObject(path, _repository, c))
                    : new JobObjectBuilder(c => new FileJobObject(path, _repository, c));
            }

            BackupsException exception = BackupsExceptionFactory.RepositoryDoesNotContainRequestedPath(_repository, path);
            _logger?.OnException(exception);
            throw exception;
        }
    }
}