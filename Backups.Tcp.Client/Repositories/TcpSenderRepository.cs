using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tcp.Dto;
using Backups.Tcp.Models;
using Backups.Tcp.RepositoryActions;
using Backups.Tcp.Tools;
using Utility.Extensions;

namespace Backups.Tcp.Client.Repositories
{
    public sealed class TcpSenderRepository : Repository, IDisposable
    {
        private readonly TcpClient _client;

        public TcpSenderRepository(ConnectionConfiguration configuration)
            : base(configuration.ToString())
        {
            _client = new TcpClient();
            _client.Connect(configuration.Host, configuration.Port);
        }

        public void Dispose()
            => _client.Dispose();

        public override bool Exists(string path)
            => ExecuteAction(new ExistsRepositoryAction(path));

        public override bool IsFolder(string path)
            => ExecuteAction(new IsFolderRepositoryAction(path));

        public override void Delete(string path)
            => ExecuteAction(new DeleteRepositoryAction(path));

        public override void Write(string path, Stream data)
            => ExecuteAction(new WriteRepositoryAction(path, data));

        public override IReadOnlyCollection<string> GetContentsOf(string folderPath)
            => ExecuteAction(new GetContentsOfRepositoryAction(folderPath));

        public override Stream GetStream(string path)
            => ExecuteAction(new GetStreamRepositoryAction(path));

        public override bool Equals(Repository? other)
            => other is TcpSenderRepository && other.Id.Equals(Id);

        public override T ExecuteAction<T>(IRepositoryAction<T> action)
        {
            action.ThrowIfNull(nameof(action));

            using NetworkStream stream = _client.GetStream();

            TcpObjectDto tcpObject = PackAction(action);
            string serializedTcpObject = JsonSerializer.Serialize(tcpObject);
            stream.Write(TcpCommunicationConfiguration.EncodeString(serializedTcpObject));

            string serializedResponse = ReadResponse(stream);

            Response responseObject = JsonSerializer
                .Deserialize<Response>(serializedResponse)
                .ThrowIfNull(nameof(Response));

            return (T)responseObject.ResponseValue!;
        }

        private static TcpObjectDto PackAction<T>(IRepositoryAction<T> action)
        {
            Type actionType = action.GetType();
            string serializedAction = JsonSerializer.Serialize(action, actionType);
            string actionTypeKey = TcpCommunicationConfiguration.GetTypeKey(actionType);

            return new TcpObjectDto(actionTypeKey, serializedAction);
        }

        private static string ReadResponse(NetworkStream stream)
        {
            byte[] data = new byte[TcpCommunicationConfiguration.DataChunkSize];
            var builder = new StringBuilder();

            do
            {
                int count = stream.Read(data, 0, data.Length);
                builder.Append(TcpCommunicationConfiguration.DecodeString(data, count));
            }
            while (stream.DataAvailable);

            string serializedResponse = builder.ToString();
            return serializedResponse;
        }
    }
}