using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Entities
{
    public sealed class Backup
    {
        private readonly List<RestorePoint> _points;
        private readonly ILogger? _logger;

        public Backup(IReadOnlyCollection<RestorePoint> points, ILogger? logger)
        {
            _points = points.ThrowIfNull(nameof(points)).ToList();
            _logger = logger;
        }

        public IReadOnlyCollection<RestorePoint> RestorePoints => _points;

        public void AddPoints(params RestorePoint[] points)
        {
            Exception[] exceptions = points
                .Where(p => _points.Contains(p))
                .Select(p => (Exception)BackupsExceptionFactory.AlreadyTrackingRestorePoint(p))
                .ToArray();

            if (exceptions.Any())
            {
                var exception = new AggregateException(exceptions);
                _logger?.OnException(exception);
                _logger?.OnComment($"The total count of already tracked points is {exceptions.Length}");
                throw exception;
            }

            _points.AddRange(points);
            _logger?.OnComment($"Backup added {points.Length} points");
        }

        public void RemovePoints(params RestorePoint[] points)
        {
            Exception[] exceptions = points
                .Where(p => !_points.Contains(p))
                .Select(p => (Exception)BackupsExceptionFactory.RestorePointIsNotBeingTracked(p))
                .ToArray();

            if (exceptions.Any())
            {
                var exception = new AggregateException(exceptions);
                _logger?.OnException(exception);
                _logger?.OnComment($"The total count of untracked points is {exceptions.Length}");
                throw exception;
            }

            foreach (RestorePoint point in points)
            {
                _points.Remove(point);
            }

            _logger?.OnComment($"Backup removed {points.Length} points");
        }
    }
}