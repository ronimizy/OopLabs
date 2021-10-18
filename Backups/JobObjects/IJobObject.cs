using System;
using Backups.Models;

namespace Backups.JobObjects
{
    public interface IJobObject : IEquatable<IJobObject>
    {
        string Name { get; }
        int Version { get; }

        Package GetPackage();
        void IncrementVersion();
    }
}