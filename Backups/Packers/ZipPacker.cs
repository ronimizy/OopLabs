using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Models;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Packers
{
    [Serializable]
    public class ZipPacker : IPacker
    {
        public Package Pack(string packageName, ILogger? logger, params Package[] packages)
        {
            packageName.ThrowIfNull(nameof(packageName));
            packages.ThrowIfNull(nameof(packages));

            var ms = new MemoryStream();
            using var archive = new ZipArchive(ms, ZipArchiveMode.Create, true);
            logger?.OnComment($"ZipPacker created a new archive for package called {packageName}");

            AddPackagesToArchive(archive, packages, logger);

            logger?.OnMessage($"ZipPacker successfully created a called {packageName}");
            return new Package($"{packageName}{BackupConfiguration.ExtensionDelimiter}zip", ms);
        }

        public Package Extract(Package package, string subPackageName, ILogger? logger)
        {
            using var archive = new ZipArchive(package.Stream, ZipArchiveMode.Read, true);
            logger?.OnComment($"ZipPacker opened archive for package {package}");

            ZipArchiveEntry entry = archive
                .GetEntry(subPackageName)
                .ThrowIfNull(BackupsExceptionFactory.MissingSubPackageException(package, subPackageName));

            return new Package(subPackageName, entry.Open());
        }

        public void AddToPackage(Stream package, ILogger? logger, params Package[] packages)
        {
            package.ThrowIfNull(nameof(package));
            packages.ThrowIfNull(nameof(packages));

            using var archive = new ZipArchive(package, ZipArchiveMode.Update, true);
            logger?.OnComment($"ZipPacker opened an archive for Package: {package}");

            AddPackagesToArchive(archive, packages, logger);
            logger?.OnMessage($"ZipPacker successfully added to a Package: {package}");
        }

        public void RemoveFromPackage(Stream package, ILogger? logger, params string[] packages)
        {
            package.ThrowIfNull(nameof(package));
            packages.ThrowIfNull(nameof(packages));

            using var archive = new ZipArchive(package, ZipArchiveMode.Update, true);
            logger?.OnComment($"ZipPacker opened an archive for Package: {package}");

            var entries = packages.Select(p => archive.GetEntry(p)).ToList();
            logger?.OnComment($"ZipPacker located entries in Package: {package}");

            if (entries.Any(e => e is null))
            {
                var exception = new ArgumentNullException(nameof(entries));
                logger?.OnException(exception, $"ZipPacker had some unexciting entries in Package: {package}");
                throw exception;
            }

            foreach (ZipArchiveEntry? entry in entries)
            {
                entry!.Delete();
            }

            logger?.OnMessage($"ZipPacker removed all requested entries from Package: {package}");
        }

        private static void AddPackagesToArchive(ZipArchive archive, IReadOnlyCollection<Package> packages, ILogger? logger)
        {
            foreach (Package obj in packages)
            {
                ZipArchiveEntry entry = archive.CreateEntry(obj.Name);
                logger?.OnComment($"Created an entry for Package: {obj}");

                using Stream entryStream = entry.Open();
                obj.Stream.Position = 0;
                obj.Stream.CopyTo(entryStream);
                logger?.OnMessage($"Package: {obj} has been added to archive");
            }
        }
    }
}