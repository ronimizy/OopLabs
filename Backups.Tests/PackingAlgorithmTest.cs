using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Packers;
using Backups.PackingAlgorithm;
using Backups.Tests.Mocks;
using Backups.Tools;
using NSubstitute;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class PackingAlgorithmTest
    {
        private const string JobName = "My Job";
        
        private InMemoryRepository _repository = null!;
        private IChronometer _chronometer = null!;
        private BackupJob _backupJob = null!;

        [SetUp]
        public void Setup()
        {
            _repository = new InMemoryRepository();
            _chronometer = Substitute.For<IChronometer>();
        }

        [Test]
        public void SingleStorageTest_FilesBackedUp_ArchiveHasFiles()
        {
            _backupJob = BackupJob.Build
                .Called(JobName)
                .UsingAlgorithm(new SingleStoragePackingAlgorithm(new ZipPacker()))
                .TrackingTimeWith(_chronometer)
                .WritingTo(_repository)
                .Build();
            
            string firstPath = $"1{BackupConfiguration.ExtensionDelimiter}txt";
            string secondPath = $"2{BackupConfiguration.ExtensionDelimiter}txt";
            var dateTime = new DateTime(2000, 1, 1);
            byte[] data = { 1, 2, 3, 4, 5 };

            _chronometer.GetCurrentTime().Returns(dateTime);
            
            _repository.Write(firstPath, new MemoryStream(data));
            _repository.Write(secondPath, new MemoryStream(data));
            
            _backupJob.AddObjects(
                _repository.GetObject.AtPath(firstPath),
                _repository.GetObject.AtPath(secondPath));
            _backupJob.Execute();

            string restorePointPath = $"{JobName}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(dateTime)}";
            string archivePath =
                $"{restorePointPath}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(dateTime)}{BackupConfiguration.ExtensionDelimiter}zip";

            Stream stream = null!;
            Assert.DoesNotThrow(
                () => stream = _repository.GetStream(archivePath));

            using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true);
            Assert.IsTrue(archive.Entries.Any(e => e.Name.Equals($"[0]_{firstPath}")));
            Assert.IsTrue(archive.Entries.Any(e => e.Name.Equals($"[0]_{secondPath}")));
        }
    }
}