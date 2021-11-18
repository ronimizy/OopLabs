using System.Collections.Generic;
using System.IO;
using Backups.Repositories;

namespace Backups.Tcp.Server.Repositories
{
    public class ConcurrentRepository : Repository
    {
        private readonly Repository _repository;
        private readonly object _lock = new object();

        public ConcurrentRepository(Repository repository)
            : base(repository.Id)
        {
            _repository = repository;
        }

        public override Repository GetSubRepositoryAt(string path)
        {
            lock (_lock)
            {
                return new ConcurrentRepository(_repository.GetSubRepositoryAt(path));
            }
        }

        public override bool Exists(string path)
        {
            lock (_lock)
            {
                return _repository.Exists(path);
            }
        }

        public override bool IsFolder(string path)
        {
            lock (_lock)
            {
                return _repository.IsFolder(path);
            }
        }

        public override void Delete(string path)
        {
            lock (_lock)
            {
                _repository.Delete(path);
            }
        }

        public override void Write(string path, Stream data)
        {
            lock (_lock)
            {
                _repository.Write(path, data);
            }
        }

        public override IReadOnlyCollection<string> GetContentsOf(string folderPath)
        {
            lock (_lock)
            {
                return _repository.GetContentsOf(folderPath);
            }
        }

        public override Stream GetStream(string path)
        {
            lock (_lock)
            {
                return _repository.GetStream(path);
            }
        }

        public override bool Equals(Repository? other)
        {
            lock (_lock)
            {
                return _repository.Equals(other);
            }
        }
    }
}