using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Repositories;
using Backups.Tests.Mocks;
using Backups.Tools;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        public static IEnumerable<TestCaseData> RepositoriesToTest
        {
            get
            {
                yield return new TestCaseData(new InMemoryRepository());
            }
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void ExistsTest_NotExisingFilePathPassed_FalseReturned(Repository repository)
        {
            string notExisingFilePath = $"file{BackupConfiguration.ExtensionDelimiter}txt";

            bool exists = repository.Exists(notExisingFilePath);

            Assert.IsFalse(exists);
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void WriteFileTest_FileWritten_FileExisting_ConfirmedFile(Repository repository)
        {
            string filePath = $"file{BackupConfiguration.ExtensionDelimiter}txt";

            repository.Write(filePath, new MemoryStream());

            Assert.IsTrue(repository.Exists(filePath));
            Assert.IsFalse(repository.IsFolder(filePath));
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void CreateFolderTest_FileWritten_FileExisting_ConfirmedFolder(Repository repository)
        {
            const string folderPath = "folder";
            string filePath = $"{folderPath}{BackupConfiguration.PathDelimiter}file{BackupConfiguration.ExtensionDelimiter}txt";

            repository.Write(filePath, new MemoryStream());

            Assert.IsTrue(repository.Exists(folderPath));
            Assert.IsTrue(repository.IsFolder(folderPath));
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void FolderWriteTest_FolderIsWrittenTo_ExceptionBeingThrown(Repository repository)
        {
            const string filePath = "folder";

            Assert.Catch<Exception>(() => repository.Write(filePath, new MemoryStream()));
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void DeleteTest_FileWrittenFileDeleted_FileDoesNotExist(Repository repository)
        {
            string filePath = $"file{BackupConfiguration.ExtensionDelimiter}txt";

            repository.Write(filePath, new MemoryStream());
            repository.Delete(filePath);

            Assert.IsFalse(repository.Exists(filePath));
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void GetContentsOfTest_FolderCreated_FilesCreated_FilesReturned(Repository repository)
        {
            const string folder = "folder";
            string[] fileNames =
            {
                $"1{BackupConfiguration.ExtensionDelimiter}txt",
                $"2{BackupConfiguration.ExtensionDelimiter}txt",
                $"3{BackupConfiguration.ExtensionDelimiter}txt",
            };
            string[] files = fileNames
                .Select(n => $"{folder}{BackupConfiguration.PathDelimiter}{n}")
                .ToArray();

            foreach (string path in files)
            {
                repository.Write(path, new MemoryStream());
            }

            IReadOnlyCollection<string> contents = repository.GetContentsOf(folder);

            foreach (string file in fileNames)
            {
                CollectionAssert.Contains(contents, file);
            }
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void GetStreamTest_WriteDataGetStream_DataEqual(Repository repository)
        {
            string filePath = $"file{BackupConfiguration.ExtensionDelimiter}txt";
            byte[] data = { 1, 2, 3, 4 };
            var ms = new MemoryStream(data);

            repository.Write(filePath, ms);
            Stream writtenStream = repository.GetStream(filePath);
            ms.SetLength(0);
            writtenStream.CopyTo(ms);

            CollectionAssert.AreEqual(data, ms.ToArray());
        }

        [TestCaseSource(nameof(RepositoriesToTest))]
        public void RepositoryRootTest_ContentsAtEmptyStringRequested_RootContentsReturned(Repository repository)
        {
            string firstPath = $"1{BackupConfiguration.ExtensionDelimiter}txt";
            string secondPath = $"2{BackupConfiguration.ExtensionDelimiter}txt";

            repository.Write(firstPath, new MemoryStream());
            repository.Write(secondPath, new MemoryStream());

            IReadOnlyCollection<string> contents = repository.GetContentsOf(string.Empty);

            CollectionAssert.Contains(contents, firstPath);
            CollectionAssert.Contains(contents, secondPath);
        }
    }
}