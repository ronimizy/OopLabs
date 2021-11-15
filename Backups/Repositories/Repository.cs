using System;
using System.Collections.Generic;
using System.IO;
using Backups.Factories;
using Backups.RepositoryActions;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Repositories
{
    public abstract class Repository : IEquatable<Repository>
    {
        protected Repository(string id, ILogger? logger = null)
        {
            Id = id;
            Logger = logger;
            GetObject = new JobObjectFactory(this, logger);
        }

        public string Id { get; }

        [JsonIgnore]
        public JobObjectFactory GetObject { get; }

        [JsonIgnore]
        protected ILogger? Logger { get; }

        public abstract Repository GetSubRepositoryAt(string path);

        public abstract bool Exists(string path);
        public abstract bool IsFolder(string path);

        public abstract void Delete(string path);
        public abstract void Write(string path, Stream data);

        public abstract IReadOnlyCollection<string> GetContentsOf(string folderPath);
        public abstract Stream GetStream(string path);

        public virtual T ExecuteAction<T>(IRepositoryAction<T> action)
        {
            Logger?.OnMessage($"Repository: {this} is about to execute a {nameof(IRepositoryAction<T>)}: {action}");
            return action.Execute(this, Logger);
        }

        public abstract bool Equals(Repository? other);

        public override bool Equals(object? obj)
            => Equals(obj as Repository);

        public override int GetHashCode()
            => Id.GetHashCode();

        public override string ToString()
            => $"[{Id}]";
    }
}