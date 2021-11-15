using System;
using System.IO;
using System.Linq;
using System.Text;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using BackupsExtra.Models;
using BackupsExtra.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace BackupsExtra.Services
{
    public class JobSavingService
    {
        public const string Extension = "bcpjb";

        public void SaveBackupJob(BackupJob job, Repository repository, string path = "")
        {
            job.ThrowIfNull(nameof(job));
            repository.ThrowIfNull(nameof(repository));
            path.ThrowIfNull(nameof(path));

            var jobObjectConfigurations = job.Objects.Select(o => o.Configuration).ToList();
            var restorePointModels = job.Points.Select(p => new RestorePointSerializationModel(p)).ToList();
            var model = new BackupJobSerializationModel(job.Configuration, jobObjectConfigurations, restorePointModels);
            string serialized = JsonConvert.SerializeObject(model);
            string name = $"{path}{job.Name}{BackupConfiguration.ExtensionDelimiter}{Extension}";

            byte[] bytes = Encoding.UTF8.GetBytes(serialized);
            using var ms = new MemoryStream(bytes);

            repository.Write(name, ms);
        }

        public BackupJob LoadBackupJob(Repository repository, string path, IChronometer chronometer, ILogger? logger = null)
        {
            repository.ThrowIfNull(nameof(repository));
            path.ThrowIfNull(nameof(path));
            chronometer.ThrowIfNull(nameof(chronometer));

            if (path.Length < Extension.Length || !path.AsSpan().EndsWith(Extension))
                throw ExceptionFactory.InvalidFileExtensionException(path, Extension);

            using Stream data = repository.GetStream(path);
            data.Position = 0;
            using var reader = new StreamReader(data);

            string serialized = reader.ReadToEnd();
            BackupJobSerializationModel serializationModel = JsonConvert
                .DeserializeObject<BackupJobSerializationModel>(serialized)
                .ThrowIfNull(nameof(BackupJobSerializationModel));
            BackupJobConfiguration configuration = serializationModel.ToConfiguration(chronometer, logger);
            var objects = serializationModel.JobObjectConfigurations
                .Select(CreateJobObjectBuilder)
                .Select(b => b.Build(configuration))
                .ToList();

            var points = serializationModel.RestorePoints
                .Select(p => CreateRestorePoint(p, configuration))
                .ToList();

            return new BackupJob(configuration, objects, points);
        }

        private JobObjectBuilder CreateJobObjectBuilder(JobObjectConfiguration configuration)
        {
            Type repositoryType = TypeLocator.Instance.Get(configuration.RepositoryConfiguration.TypeKey);
            Repository repository = (Repository)JsonConvert
                .DeserializeObject(configuration.RepositoryConfiguration.Data, repositoryType)
                .ThrowIfNull(nameof(Repository));

            return repository.GetObject.AtPath(configuration.Path);
        }

        private RestorePoint CreateRestorePoint(RestorePointSerializationModel restorePointModel, BackupJobConfiguration jobConfiguration)
        {
            Type repositoryType = TypeLocator.Instance.Get(restorePointModel.RepositoryConfiguration.TypeKey);
            var repository = (Repository)JsonConvert
                .DeserializeObject(restorePointModel.RepositoryConfiguration.Data, repositoryType)
                .ThrowIfNull(nameof(Repository));
            var objects = restorePointModel.Objects
                .Select(CreateJobObjectBuilder)
                .Select(b => b.Build(jobConfiguration))
                .ToList();

            return new RestorePoint(repository, restorePointModel.CreatedDateTime, objects);
        }
    }
}