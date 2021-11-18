using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Models;
using Backups.Repositories;
using Newtonsoft.Json;
using Utility.Extensions;

namespace BackupsExtra.Models
{
    public class RestorePointSerializationModel
    {
        public RestorePointSerializationModel(RestorePoint point)
        {
            point.ThrowIfNull(nameof(point));

            RepositoryConfiguration = new SerializedConfiguration<Repository>(point.Repository);
            CreatedDateTime = point.CreatedDateTime;
            Objects = point.Objects.Select(o => o.Configuration).ToList();
        }

        [JsonConstructor]
        private RestorePointSerializationModel(
            SerializedConfiguration<Repository> repositoryConfiguration,
            DateTime createdDateTime,
            IReadOnlyCollection<JobObjectConfiguration> objects)
        {
            RepositoryConfiguration = repositoryConfiguration.ThrowIfNull(nameof(repositoryConfiguration));
            CreatedDateTime = createdDateTime.ThrowIfNull(nameof(createdDateTime));
            Objects = objects.ThrowIfNull(nameof(objects));
        }

        public SerializedConfiguration<Repository> RepositoryConfiguration { get; }
        public DateTime CreatedDateTime { get; }
        public IReadOnlyCollection<JobObjectConfiguration> Objects { get; }
    }
}