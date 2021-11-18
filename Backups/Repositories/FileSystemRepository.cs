using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Tools;

namespace Backups.Repositories
{
    public class FileSystemRepository : Repository
    {
        public FileSystemRepository(string id)
            : base(id) { }

        public override Repository GetSubRepositoryAt(string path)
            => new FileSystemRepository($"{Id}{BackupConfiguration.PathDelimiter}{path}");

        public override bool Exists(string path)
            => File.Exists(GetFullPath(path)) || Directory.Exists(GetFullPath(path));

        public override bool IsFolder(string path)
            => Directory.Exists(GetFullPath(path));

        public override void Delete(string path)
            => File.Delete(GetFullPath(path));

        public override void Write(string path, Stream data)
        {
            path = GetFullPath(path);
            string folderPath = Path.GetDirectoryName(path) ?? string.Empty;

            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            data.Position = 0;
            data.CopyTo(fs);
        }

        public override IReadOnlyCollection<string> GetContentsOf(string folderPath)
            => Directory.GetFiles(GetFullPath(folderPath)).Concat(Directory.GetDirectories(GetFullPath(folderPath))).ToList();

        public override Stream GetStream(string path)
            => new FileStream(GetFullPath(path), FileMode.OpenOrCreate, FileAccess.ReadWrite);

        public override bool Equals(Repository? other)
            => other is FileSystemRepository && other.Id.Equals(Id);

        private string GetFullPath(string path)
            => Path.Combine(Id, Path.GetFullPath(path));
    }
}