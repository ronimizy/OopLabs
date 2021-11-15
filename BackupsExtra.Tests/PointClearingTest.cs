using System;
using System.IO;
using System.Linq;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Packers;
using Backups.StorageAlgorithms;
using Backups.Tests.Mocks;
using Backups.Tests.Tools;
using Backups.Tools;
using BackupsExtra.Filters;
using BackupsExtra.Loggers;
using BackupsExtra.Matchers;
using NSubstitute;
using NUnit.Framework;
using Spectre.Console;

namespace BackupsExtra.Tests
{
    [TestFixture]
    public class PointClearingTest
    {
        private const string Name = "My Job";
        private const string BackupsRepositoryName = "backups";

        private const string PathOne = "One.txt";
        private const string PathTwo = "Two.txt";
        private const string PathThree = "Three.txt";

        private InMemoryRepository _repository;
        private IChronometer _chronometer;

        [SetUp]
        public void Setup()
        {
            _repository = new InMemoryRepository(BackupsRepositoryName);
            _chronometer = Substitute.For<IChronometer>();
        }

        [TearDown]
        public void Teardown()
        {
            InMemoryRepository.Clear();
        }

        [Test]
        public void MergingPointAmountLimitedTest_RestorePointsCreated_RestorePointsMerged()
        {
            BackupJob job = BackupJob.Build
                .Called(Name)
                .PackingWith(new ZipPacker())
                .UsingAlgorithm(new SplitStorageStorageAlgorithm())
                .TrackingTimeWith(_chronometer)
                .WritingTo(_repository)
                .WithRestorePointFilteringPolicy(new MergeRestorePointFilter(), new PointCountRestorePointMatcher(1))
                .Build();

            var firstDate = new DateTime(2020, 10, 20);
            var secondDate = new DateTime(2020, 10, 21);

            var readingRepository = new InMemoryRepository("data");
            readingRepository.Write(PathOne, new MemoryStream());
            readingRepository.Write(PathTwo, new MemoryStream());
            readingRepository.Write(PathThree, new MemoryStream());

            var objectOne = readingRepository.GetObject.AtPath(PathOne);
            var objectTwo = readingRepository.GetObject.AtPath(PathTwo);
            var objectThree = readingRepository.GetObject.AtPath(PathThree);

            _chronometer.GetCurrentTime().Returns(firstDate);
            job.AddObjects(objectOne, objectTwo);
            job.Execute();

            _chronometer.GetCurrentTime().Returns(secondDate);
            job.RemoveObjects(job.Objects.First());
            job.AddObjects(objectThree);
            job.Execute();

            AnsiConsole.Write(InMemoryRepositoryComposer.Compose());

            string secondBackupPath = $"{Name}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(secondDate)}";

            Assert.IsTrue(_repository.Exists(secondBackupPath));
            Assert.IsFalse(_repository.Exists($"{Name}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(firstDate)}"));
            Assert.AreEqual(3, _repository.GetContentsOf(secondBackupPath).Count);
        }
        
        [Test]
        public void ObjectLimitDeleteTest_RestorePointsCreated_RestorePointsMerged()
        {
            BackupJob job = BackupJob.Build
                .Called(Name)
                .PackingWith(new ZipPacker())
                .UsingAlgorithm(new SplitStorageStorageAlgorithm())
                .TrackingTimeWith(_chronometer)
                .WritingTo(_repository)
                .WithRestorePointFilteringPolicy(new DeleteRestorePointFilter(), new ObjectCountRestorePointMatcher(1))
                .LoggingWith(new ConsoleLogger())
                .Build();

            var firstDate = new DateTime(2020, 10, 20);
            var secondDate = new DateTime(2020, 10, 21);

            var readingRepository = new InMemoryRepository("data");
            readingRepository.Write(PathOne, new MemoryStream());

            var objectOne = readingRepository.GetObject.AtPath(PathOne);

            _chronometer.GetCurrentTime().Returns(firstDate);
            job.AddObjects(objectOne);
            job.Execute();

            _chronometer.GetCurrentTime().Returns(secondDate);
            job.Execute();

            AnsiConsole.Write(InMemoryRepositoryComposer.Compose());

            string secondBackupPath = $"{Name}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(secondDate)}";

            Assert.IsTrue(_repository.Exists(secondBackupPath));
            Assert.IsFalse(_repository.Exists($"{Name}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(firstDate)}"));
            Assert.AreEqual(1, _repository.GetContentsOf(secondBackupPath).Count);
        }
    }
}