using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Models;
using Backups.Packers;
using Backups.Repositories;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.StorageAlgorithms;
using Backups.Tests.Mocks;
using Backups.Tests.Tools;
using Backups.Tools;
using NSubstitute;
using NUnit.Framework;
using Spectre.Console;

namespace Backups.Tests
{
    [TestFixture]
    public class BackupJobTests
    {
        private const string JobName = "My Backup Job";

        private BackupJob _backupJob = null!;
        private IStorageAlgorithm _storageAlgorithm = null!;
        private IChronometer _chronometer = null!;
        private InMemoryRepository _repository = null!;
        private IRestorePointFilter _pointFilter = null!;
        private IRestorePointMatcher _pointMatcher = null!;

        [SetUp]
        public void Setup()
        {
            _storageAlgorithm = new SplitStorageStorageAlgorithm();
            _chronometer = Substitute.For<IChronometer>();
            _repository = new InMemoryRepository();
            _pointFilter = Substitute.For<IRestorePointFilter>();
            _pointMatcher = Substitute.For<IRestorePointMatcher>();

            _backupJob = BackupJob.Build
                .Called(JobName)
                .PackingWith(new ZipPacker())
                .UsingAlgorithm(_storageAlgorithm)
                .TrackingTimeWith(_chronometer)
                .WritingTo(_repository)
                .WithRestorePointFilteringPolicy(_pointFilter, _pointMatcher)
                .Build();
        }

        [Test]
        public void ExecuteTest_BackupCreationRequested_RestorePointsArePacked_DataIsWrittenToRepository()
        {
            string firstPath = $"1{BackupConfiguration.ExtensionDelimiter}txt";
            string secondPath = $"2{BackupConfiguration.ExtensionDelimiter}txt";
            var dateTimeFirst = new DateTime(2000, 1, 1);
            var dateTimeSecond = new DateTime(2000, 1, 2);

            _chronometer.GetCurrentTime().Returns(dateTimeFirst);

            _repository.Write(firstPath, new MemoryStream());
            _repository.Write(secondPath, new MemoryStream());

            JobObjectBuilder[] objects = { _repository.GetObject.AtPath(firstPath), _repository.GetObject.AtPath(secondPath) };

            _backupJob.AddObjects(objects);
            _backupJob.Execute();

            _chronometer.GetCurrentTime().Returns(dateTimeSecond);

            _backupJob.RemoveObjects(_backupJob.Objects.First());
            _backupJob.Execute();

            _pointFilter.Received(2).Filter(
                Arg.Any<Backup>(), _pointMatcher, _storageAlgorithm, _backupJob.Configuration.Packer, Arg.Any<Repository>(), Arg.Any<ILogger>());

            AnsiConsole.Write(InMemoryRepositoryComposer.Compose());

            string firstRestorePointName = dateTimeFirst.ToString("u");
            string secondRestorePointName = dateTimeSecond.ToString("u");

            IReadOnlyCollection<string> firstContents = null!;
            IReadOnlyCollection<string> secondContents = null!;

            Assert.DoesNotThrow(() => firstContents = _repository
                                    .GetContentsOf($"{JobName}{BackupConfiguration.PathDelimiter}{firstRestorePointName}"));
            Assert.DoesNotThrow(() => secondContents = _repository
                                    .GetContentsOf($"{JobName}{BackupConfiguration.PathDelimiter}{secondRestorePointName}"));

            CollectionAssert.Contains(firstContents, $"{firstPath}{BackupConfiguration.ExtensionDelimiter}zip");
            CollectionAssert.Contains(firstContents, $"{secondPath}{BackupConfiguration.ExtensionDelimiter}zip");

            CollectionAssert.Contains(secondContents, $"{secondPath}{BackupConfiguration.ExtensionDelimiter}zip");
            CollectionAssert.DoesNotContain(secondContents, $"{firstPath}{BackupConfiguration.ExtensionDelimiter}zip");
            CollectionAssert.DoesNotContain(secondContents, $"{firstPath}{BackupConfiguration.ExtensionDelimiter}zip");
        }
    }
}