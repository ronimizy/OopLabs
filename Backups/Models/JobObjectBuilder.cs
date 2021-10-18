using System;
using Backups.JobObjects;
using Utility.Extensions;

namespace Backups.Models
{
    public class JobObjectBuilder
    {
        private readonly Func<IServiceProvider, IJobObject> _creator;

        internal JobObjectBuilder(Func<IServiceProvider, IJobObject> creator)
        {
            _creator = creator.ThrowIfNull(nameof(creator));
        }

        internal IJobObject Create(IServiceProvider provider)
            => _creator(provider);
    }
}