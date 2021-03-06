using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Backups.Repositories;
using Backups.Tcp.Server.Processors;
using Backups.Tcp.Server.Repositories;
using Backups.Tcp.Tools;
using Backups.Tools;

namespace Backups.Tcp.Server.Receivers
{
    public class TcpReceiver
    {
        private readonly Repository _repository;
        private readonly TcpListener _listener;
        private readonly TypeLocator _locator;
        private readonly ILogger? _logger;
        private readonly List<Thread> _processorThreads;
        private readonly List<RepositoryActionProcessor> _processors;

        public TcpReceiver(ConnectionConfiguration configuration, Repository repository, TypeLocator locator, ILogger? logger = null)
        {
            _locator = locator;
            _logger = logger;
            _repository = new ConcurrentRepository(repository);
            _listener = new TcpListener(IPAddress.Parse(configuration.Host), configuration.Port);
            _processorThreads = new List<Thread>();
            _processors = new List<RepositoryActionProcessor>();
        }

        public void Run()
        {
            try
            {
                _listener.Start();

                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    var processor = new RepositoryActionProcessor(client, _repository, _locator, _logger);

                    var thread = new Thread(processor.Run);
                    _processorThreads.Add(thread);
                    _processors.Add(processor);
                    thread.Start();
                }
            }
            catch (Exception e)
            {
                _logger?.OnException(e);
            }
            finally
            {
                _listener.Stop();
                foreach (var (thread, processor) in _processorThreads.Zip(_processors))
                {
                    processor.Stop();
                    thread.Join();
                }
            }
        }

        public void Stop()
            => _listener.Stop();
    }
}