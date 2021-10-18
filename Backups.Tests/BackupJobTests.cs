using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Models;
using Backups.Packers;
using Backups.PackingAlgorithm;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
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
        private IPackingAlgorithm _packingAlgorithm = null!;
        private IChronometer _chronometer = null!;
        private InMemoryRepository _repository = null!;
        private IRestorePointFilter _pointFilter = null!;
        private IRestorePointMatcher _pointMatcher = null!;

        [SetUp]
        public void Setup()
        {
            _packingAlgorithm = new SplitStoragePackingAlgorithm(new ZipPacker());
            _chronometer = Substitute.For<IChronometer>();
            _repository = new InMemoryRepository();
            _pointFilter = Substitute.For<IRestorePointFilter>();
            _pointMatcher = Substitute.For<IRestorePointMatcher>();

            _backupJob = BackupJob.Build
                .Called(JobName)
                .UsingAlgorithm(_packingAlgorithm)
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

            _pointFilter.Received(2).Filter(Arg.Any<Backup>(), _pointMatcher, _packingAlgorithm, _repository, Arg.Any<ILogger>());

            AnsiConsole.Write(new InMemoryRepositoryComposer(_repository).Compose());

            string firstRestorePointName = dateTimeFirst.ToString("u");
            string secondRestorePointName = dateTimeSecond.ToString("u");

            IReadOnlyCollection<string> firstContents = null!;
            IReadOnlyCollection<string> secondContents = null!;

            Assert.DoesNotThrow(() => firstContents = _repository
                                    .GetContentsOf($"{JobName}{BackupConfiguration.PathDelimiter}{firstRestorePointName}"));
            Assert.DoesNotThrow(() => secondContents = _repository
                                    .GetContentsOf($"{JobName}{BackupConfiguration.PathDelimiter}{secondRestorePointName}"));

            CollectionAssert.Contains(firstContents, $"[0]_{firstPath}{BackupConfiguration.ExtensionDelimiter}zip");
            CollectionAssert.Contains(firstContents, $"[0]_{secondPath}{BackupConfiguration.ExtensionDelimiter}zip");

            CollectionAssert.Contains(secondContents, $"[1]_{secondPath}{BackupConfiguration.ExtensionDelimiter}zip");
            CollectionAssert.DoesNotContain(secondContents, $"[1]_{firstPath}{BackupConfiguration.ExtensionDelimiter}zip");
            CollectionAssert.DoesNotContain(secondContents, $"[0]_{firstPath}{BackupConfiguration.ExtensionDelimiter}zip");
        }
    }
}