using System;
using System.IO;
using System.Threading;
using Backups.Chronometers;
using Backups.Entities;
using Backups.Packers;
using Backups.PackingAlgorithm;
using Backups.RestorePointFilters;
using Backups.RestorePointMatchers;
using Backups.Tcp.Client.Repositories;
using Backups.Tcp.Extensions;
using Backups.Tcp.Server.Receivers;
using Backups.Tcp.Tools;
using Backups.Tests.Mocks;
using Backups.Tests.Tools;
using Backups.Tools;
using NSubstitute;
using NUnit.Framework;
using Spectre.Console;

namespace Backups.Tests
{
    [TestFixture]
    public sealed class TcpTest : IDisposable
    {
        private const string JobName = "My Job";
        
        private InMemoryRepository _readingRepository = null!;
        private InMemoryRepository _writingRepository = null!;
        private IChronometer _chronometer = null!;
        private IRestorePointFilter _filter = null!;
        private IRestorePointMatcher _matcher = null!;

        private TcpSenderRepository _sender = null!;
        private TcpReceiver _receiver = null!;
        private Thread _receiverThread = null!;

        private BackupJob _backupJob = null!;

        [SetUp]
        public void Setup()
        {
            const string ip = "127.0.0.1";

            var configuration = new ConnectionConfiguration(ip, 8888);
            TypeLocator locator = new TypeLocator()
                .AddPackers()
                .AddRepositories()
                .AddPackingAlgorithms()
                .AddRepositoryActions();

            _readingRepository = new InMemoryRepository();
            _writingRepository = new InMemoryRepository();
            _chronometer = Substitute.For<IChronometer>();
            _filter = Substitute.For<IRestorePointFilter>();
            _matcher = Substitute.For<IRestorePointMatcher>();

            _receiver = new TcpReceiver(configuration, _writingRepository, locator);
            _receiverThread = new Thread(_receiver.Run);
            _receiverThread.Start();
            _sender = new TcpSenderRepository(configuration);

            _backupJob = BackupJob.Build
                .Called(JobName)
                .UsingAlgorithm(new SplitStoragePackingAlgorithm(new ZipPacker()))
                .TrackingTimeWith(_chronometer)
                .WritingTo(_sender)
                .WithRestorePointFilteringPolicy(_filter, _matcher)
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            _sender.Dispose();
            _receiverThread.Join();
        }

        [Test]
        public void TcpBackupJobTest_ObjectsTracked_JobExecuted_ObjectSent()
        {
            string firstPath = $"1{BackupConfiguration.ExtensionDelimiter}txt";
            string secondPath = $"2{BackupConfiguration.ExtensionDelimiter}txt";
            byte[] data = { 1, 2, 3, 4, 5 };
            var dateTime = new DateTime(2000, 1, 1);
            
            _readingRepository.Write(firstPath, new MemoryStream(data));
            _readingRepository.Write(secondPath, new MemoryStream(data));

            _chronometer.GetCurrentTime().Returns(dateTime);
            
            _backupJob.AddObjects(
                _readingRepository.GetObject.AtPath(firstPath),
                _readingRepository.GetObject.AtPath(secondPath));
            _backupJob.Execute();

            string restorePointPath = $"{JobName}{BackupConfiguration.PathDelimiter}{BackupConfiguration.FormatDateTime(dateTime)}";
            string firstWrittenPath = $"{restorePointPath}{BackupConfiguration.PathDelimiter}[0]_{firstPath}{BackupConfiguration.ExtensionDelimiter}zip";
            string secondWrittenPath = $"{restorePointPath}{BackupConfiguration.PathDelimiter}[0]_{secondPath}{BackupConfiguration.ExtensionDelimiter}zip";
            
            Assert.IsTrue(_writingRepository.Exists(JobName));
            Assert.IsTrue(_writingRepository.IsFolder(JobName));
            
            Assert.IsTrue(_writingRepository.Exists(restorePointPath));
            Assert.IsTrue(_writingRepository.IsFolder(restorePointPath));
            
            Assert.IsTrue(_writingRepository.Exists(firstWrittenPath));
            Assert.IsTrue(_writingRepository.Exists(secondWrittenPath));
            
            AnsiConsole.WriteLine(nameof(_readingRepository));
            AnsiConsole.Write(new InMemoryRepositoryComposer(_readingRepository).Compose());
            
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine(nameof(_writingRepository));
            AnsiConsole.Write(new InMemoryRepositoryComposer(_writingRepository).Compose());

            _receiver.Stop();
        }

        public void Dispose()
            => _sender.Dispose();
    }
}