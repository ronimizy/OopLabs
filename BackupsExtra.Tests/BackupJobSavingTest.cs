using System;
using System.IO;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Models;
using Backups.Packers;
using Backups.StorageAlgorithms;
using Backups.Tests.Mocks;
using Backups.Tools;
using BackupsExtra.Extensions;
using BackupsExtra.Filters;
using BackupsExtra.Matchers;
using BackupsExtra.Services;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    [TestFixture]
    public class BackupJobSavingTest
    {
        private const string ObjectsPath = "obj";
        private const string BackupsPath = "backups";
        private const string SavingPath = "saves";

        private const string PathOne = "One.txt";
        private static readonly string PathTwo = $"Folder{BackupConfiguration.PathDelimiter}Two.txt";

        private JobSavingService _savingService;
        private InMemoryRepository _objectRepository;
        private InMemoryRepository _backupsRepository;
        private InMemoryRepository _jobSavingRepository;

        private IChronometer _chronometer;

        [SetUp]
        public void Setup()
        {
            TypeLocator.Instance
                .Add(typeof(InMemoryRepository))
                .Add(typeof(ZipPacker))
                .Add(typeof(SplitStorageStorageAlgorithm))
                .AddMatchers()
                .AddFilters();

            _savingService = new JobSavingService();
            _objectRepository = new InMemoryRepository(ObjectsPath);
            _backupsRepository = new InMemoryRepository(BackupsPath);
            _jobSavingRepository = new InMemoryRepository(SavingPath);
            _chronometer = new CurrentTimeUtcChronometer();

            _objectRepository.Write(PathOne, new MemoryStream());
            _objectRepository.Write(PathTwo, new MemoryStream());
        }

        [Test]
        public void JobSavingTest_JobSaved_JobLoaded()
        {
            var matcher = new AnyOfRestorePointMatcher(
                new DateTimeRestorePointMatcher(DateTime.Now),
                new PointCountRestorePointMatcher(2));

            BackupJob job = BackupJob.Build
                .Called("My Job")
                .PackingWith(new ZipPacker())
                .UsingAlgorithm(new SplitStorageStorageAlgorithm())
                .TrackingTimeWith(_chronometer)
                .WritingTo(_backupsRepository)
                .WithRestorePointFilteringPolicy(new MergeRestorePointFilter(), matcher)
                .Build();

            job.AddObjects(_objectRepository.GetObject.AtPath(PathOne), _objectRepository.GetObject.AtPath(PathTwo));
            job.Execute();

            _savingService.SaveBackupJob(job, _jobSavingRepository);
            BackupJob loadedJob = _savingService.LoadBackupJob(
                _jobSavingRepository, $"{job.Name}{BackupConfiguration.ExtensionDelimiter}{JobSavingService.Extension}", _chronometer);

            BackupJobConfiguration loadedConfiguration = loadedJob.Configuration;

            Assert.AreEqual(job.Name, loadedJob.Name);
            Assert.AreEqual(2, loadedJob.Objects.Count);
            Assert.AreEqual(1, loadedJob.Points.Count);
            Assert.NotNull(loadedConfiguration.Packer);
            Assert.NotNull(loadedConfiguration.StorageAlgorithm);
            Assert.NotNull(loadedConfiguration.Chronometer);
            Assert.NotNull(loadedConfiguration.WritingRepository);
            Assert.NotNull(loadedConfiguration.RestorePointFilter);
            Assert.NotNull(loadedConfiguration.RestorePointMatcher);
            Assert.True(loadedConfiguration.RestorePointMatcher is AnyOfRestorePointMatcher);
            Assert.AreEqual(2, (loadedConfiguration.RestorePointMatcher as AnyOfRestorePointMatcher)?.Matchers.Count);
        }
    }
}