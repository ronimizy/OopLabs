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
            _repository = repository;
            _logger = logger;
        }

        public JobObjectBuilder AtPath(string path)
        {
            path.ThrowIfNull(nameof(path));

            if (_repository.Exists(path))
            {
                _logger?.OnComment($"JobObjectBuilder for path {path} at repository {_repository} is being created");

                return _repository.IsFolder(path)
                    ? new JobObjectBuilder(p => new FolderJobObject(path, _repository, this, p))
                    : new JobObjectBuilder(p => new FileJobObject(path, _repository, p));
            }

            BackupsException exception = BackupsExceptionFactory.RepositoryDoesNotContainRequestedPath(_repository, path);
            _logger?.OnException(exception);
            throw exception;
        }
    }
}