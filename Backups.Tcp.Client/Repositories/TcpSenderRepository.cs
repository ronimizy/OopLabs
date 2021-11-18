using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tcp.Dto;
using Backups.Tcp.Models;
using Backups.Tcp.RepositoryActions;
using Backups.Tcp.Tools;
using Backups.Tools;
using Newtonsoft.Json;
using Utility.Extensions;

namespace Backups.Tcp.Client.Repositories
{
    public sealed class TcpSenderRepository : Repository, IDisposable
    {
        private readonly TcpClient _client;
        private readonly ConnectionConfiguration _configuration;
        private readonly string _basePath;

        public TcpSenderRepository(ConnectionConfiguration configuration)
            : base(configuration.ToString())
        {
            _configuration = configuration;
            _client = new TcpClient();
            _client.Connect(configuration.Host, configuration.Port);
            _basePath = string.Empty;
        }

        private TcpSenderRepository(ConnectionConfiguration configuration, string basePath)
            : this(configuration)
        {
            _basePath = basePath;
        }

        public void Dispose()
            => _client.Dispose();

        public override Repository GetSubRepositoryAt(string path)
            => new TcpSenderRepository(_configuration, AddBasePath(path));

        public override bool Exists(string path)
            => ExecuteAction(new ExistsRepositoryAction(AddBasePath(path)));

        public override bool IsFolder(string path)
            => ExecuteAction(new IsFolderRepositoryAction(AddBasePath(path)));

        public override void Delete(string path)
            => ExecuteAction(new DeleteRepositoryAction(AddBasePath(path)));

        public override void Write(string path, Stream data)
            => ExecuteAction(new WriteRepositoryAction(AddBasePath(path), data));

        public override IReadOnlyCollection<string> GetContentsOf(string folderPath)
            => ExecuteAction(new GetContentsOfRepositoryAction(AddBasePath(folderPath)));

        public override Stream GetStream(string path)
            => ExecuteAction(new GetStreamRepositoryAction(AddBasePath(path)));

        public override bool Equals(Repository? other)
            => other is TcpSenderRepository tcpSenderRepository &&
               other.Id.Equals(Id) &&
               _basePath.Equals(tcpSenderRepository._basePath);

        public override T ExecuteAction<T>(IRepositoryAction<T> action)
        {
            action.ThrowIfNull(nameof(action));

            NetworkStream stream = _client.GetStream();

            TcpObjectDto tcpObject = PackAction(action);
            string serializedTcpObject = JsonConvert.SerializeObject(tcpObject);
            stream.Write(BitConverter.GetBytes(serializedTcpObject.Length));
            stream.Write(TcpCommunicationConfiguration.EncodeString(serializedTcpObject));

            string serializedResponse = ReadResponse(stream);

            Response responseObject = JsonConvert
                .DeserializeObject<Response>(serializedResponse)
                .ThrowIfNull(nameof(Response));

            return (T)responseObject.ResponseValue!;
        }

        private static TcpObjectDto PackAction<T>(IRepositoryAction<T> action)
        {
            Type actionType = action.GetType();
            string serializedAction = JsonConvert.SerializeObject(action, actionType, null);
            string actionTypeKey = TcpCommunicationConfiguration.GetTypeKey(actionType);

            return new TcpObjectDto(actionTypeKey, serializedAction);
        }

        private static string ReadResponse(NetworkStream stream)
        {
            Decoder decoder = TcpCommunicationConfiguration.Encoding.GetDecoder();
            var builder = new StringBuilder();
            byte[] data = new byte[TcpCommunicationConfiguration.DataChunkSize];
            char[] chars = new char[TcpCommunicationConfiguration.DataChunkSize];

            int sizeRead = 0;
            while (sizeRead != sizeof(int))
            {
                sizeRead += stream.Read(data, sizeRead, sizeof(int) - sizeRead);
            }

            int dataSize = BitConverter.ToInt32(data.AsSpan(0, sizeof(int)));

            while (dataSize > 0)
            {
                int count = stream.Read(data, 0, Math.Min(data.Length, dataSize));
                int charCount = decoder.GetChars(data, 0, count, chars, 0);
                builder.Append(chars.AsSpan(0, charCount));
                dataSize -= count;
            }

            return builder.ToString();
        }

        private string AddBasePath(string path)
            => $"{_basePath}{BackupConfiguration.PathDelimiter}{path}";
    }
}