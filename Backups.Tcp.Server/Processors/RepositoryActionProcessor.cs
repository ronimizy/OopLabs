using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Backups.Repositories;
using Backups.RepositoryActions;
using Backups.Tcp.Dto;
using Backups.Tcp.Models;
using Backups.Tcp.Tools;
using Backups.Tools;
using Newtonsoft.Json;
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

                while (true)
                {
                    IRepositoryAction<object?> action = UnpackAction(stream);

                    object? response = _repository.ExecuteAction(action);
                    var responseObject = new Response(Status.Success, response);
                    string serializedResponse = JsonConvert.SerializeObject(responseObject);

                    stream.Write(BitConverter.GetBytes(serializedResponse.Length));
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

        public void Stop()
            => _client.Close();

        private IRepositoryAction<object?> UnpackAction(Stream stream)
        {
            var builder = new StringBuilder();
            Decoder decoder = TcpCommunicationConfiguration.Encoding.GetDecoder();
            byte[] bytes = new byte[TcpCommunicationConfiguration.DataChunkSize];
            char[] chars = new char[TcpCommunicationConfiguration.DataChunkSize];

            int sizeRead = 0;
            while (sizeRead != sizeof(int))
            {
                sizeRead += stream.Read(bytes, sizeRead, sizeof(int) - sizeRead);
            }

            int contentSize = BitConverter.ToInt32(bytes.AsSpan(0, sizeof(int)));

            while (contentSize > 0)
            {
                int read = stream.Read(bytes, 0, Math.Min(bytes.Length, contentSize));
                int charCount = decoder.GetChars(bytes, 0, read, chars, 0);
                builder.Append(chars.AsSpan(0, charCount));
                contentSize -= read;
            }

            string serialized = builder.ToString();

            TcpObjectDto dto = JsonConvert
                .DeserializeObject<TcpObjectDto>(serialized)
                .ThrowIfNull(nameof(TcpObjectDto));

            var deserialized = (IRepositoryAction<object?>)JsonConvert
                .DeserializeObject(dto.Data, _locator.Get(dto.Type))
                .ThrowIfNull(nameof(IRepositoryAction<object?>));

            return deserialized;
        }
    }
}