using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tcp.Dto;
using Backups.Tcp.Models;
using Backups.Tcp.Tools;
using Backups.Tools;
using Utility.Extensions;

namespace Backups.Tcp.Server.Processors
{
    public class RepositoryActionProcessor
    {
        private readonly TcpClient _client;
        private readonly Repository _repository;
        private readonly TypeLocator _locator;
        private readonly ILogger? _logger;

        public RepositoryActionProcessor(TcpClient client, Repository repository, TypeLocator locator, ILogger? logger)
        {
            _client = client;
            _repository = repository;
            _locator = locator;
            _logger = logger;
        }

        public void Run()
        {
            NetworkStream? stream = null;
            try
            {
                stream = _client.GetStream();
                byte[] bytes = new byte[TcpCommunicationConfiguration.DataChunkSize];

                while (true)
                {
                    IRepositoryAction<object?> action = UnpackAction(stream, bytes);

                    object? response = _repository.ExecuteAction(action);
                    var responseObject = new Response(Status.Success, response);
                    string serializedResponse = JsonSerializer.Serialize(responseObject);

                    stream.Write(TcpCommunicationConfiguration.EncodeString(serializedResponse));
                }
            }
            catch (Exception e)
            {
                _logger?.OnException(e);
            }
            finally
            {
                stream?.Close();
                _client.Close();
            }
        }

        private IRepositoryAction<object?> UnpackAction(NetworkStream stream, byte[] bytes)
        {
            var builder = new StringBuilder();

            do
            {
                int count = stream.Read(bytes, 0, bytes.Length);
                builder.Append(TcpCommunicationConfiguration.DecodeString(bytes, count));
            }
            while (stream.DataAvailable);

            string serialized = builder.ToString();

            TcpObjectDto dto = JsonSerializer
                .Deserialize<TcpObjectDto>(serialized)
                .ThrowIfNull(nameof(TcpObjectDto));

            var deserialized = (IRepositoryAction<object?>)JsonSerializer
                .Deserialize(dto.Data, _locator.Get(dto.Type))
                .ThrowIfNull(nameof(IRepositoryAction<object?>));

            return deserialized;
        }
    }
}