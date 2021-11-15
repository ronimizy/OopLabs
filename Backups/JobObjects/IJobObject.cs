using System;
using System.IO;
using Backups.Models;
using Backups.Repositories;

namespace Backups.JobObjects
{
    public interface IJobObject : IEquatable<IJobObject>
    {
        string Name { get; }
        string FullPath { get; }
        JobObjectConfiguration Configuration { get; }
        Repository Repository { get; }

        Stream GetStream();
    }
}