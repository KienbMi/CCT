using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipe
{
    public abstract class BaseNamedPipe
    {
        protected string _input;
        protected string _output;
        protected string _pipeName = "myPipeName";

        protected StreamReader _reader;
        protected StreamWriter _writer;

        protected FixedSizedQueue<string> _errorBuffer;
        protected bool _errorToConsole;
        protected BaseNamedPipe(bool errorToConsole, string pipeName)
        {
            _errorToConsole = errorToConsole;
            _errorBuffer = new FixedSizedQueue<string>(500);

            if (pipeName != null)
            {
                _pipeName = pipeName;
            }
        }
    }

    public class PipeServer : BaseNamedPipe
    {
        NamedPipeServerStream _server;
        string _receivedMessage;

        Task _serverTask;
        CancellationTokenSource _tokenSource;
        CancellationToken _ct;

        public PipeServer(bool errorToConsole = false, string pipeName = null) : base(errorToConsole, pipeName)
        {
            Start();
        }

        public void SendMessage(string message)
        {
            _output = message;
        }

        public string ReceiceMessage()
        {
            string result = _receivedMessage;
            _receivedMessage = "";
            return result;
        }

        public async Task StopAsync()
        {
            _tokenSource?.Cancel();

            // Just continue on this thread, or await with try-catch:
            try
            {
                await _serverTask;
            }
            catch (OperationCanceledException e)
            {
                if (_errorToConsole)
                {
                    Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
                }
            }
            finally
            {
                _tokenSource.Dispose();
            }
        }

        private void Start()
        {
            _tokenSource = new CancellationTokenSource();
            _ct = _tokenSource.Token;

            _serverTask = Task.Run(() =>
            {
                // Were we already canceled?
                _ct.ThrowIfCancellationRequested();

                InitServer();

                while (true)
                {
                    if (_server.IsConnected)
                    {
                        try
                        {
                            _input = _reader.ReadLine();
                            if (!string.IsNullOrEmpty(_input))
                            {
                                _receivedMessage = _input;
                            }

                            _writer.WriteLine(_output);
                            _writer.Flush();
                            _output = "";
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = $"Pipe Server error: {ex.Message}";
                            _errorBuffer.Enqueue(errorMessage);

                            if (_errorToConsole)
                            {
                                Console.WriteLine(errorMessage);
                            }
                        }
                    }
                    else
                    {
                        InitServer();
                    }

                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (_ct.IsCancellationRequested)
                    {
                        _server?.Close();
                        _server?.Dispose();
                        _ct.ThrowIfCancellationRequested();
                    }

                }
            }, _tokenSource.Token);
        }

        private void InitServer()
        {
            _server?.Close();
            _server?.Dispose();

            _server = new NamedPipeServerStream(_pipeName, PipeDirection.InOut);
            _server.WaitForConnection();
            _reader = new StreamReader(_server);
            _writer = new StreamWriter(_server);
        }
    }

    public class PipeClient : BaseNamedPipe
    {
        NamedPipeClientStream _client;
        int _timeout = 1000; // 1000ms

        public PipeClient(bool errorToConsole = false, string pipeName = null) : base(errorToConsole, pipeName)
        {
        }

        public void SendMessage(string message)
        {
            _output = message;
            Start();
        }

        public string ReceiceMessage()
        {
            Start();
            return (_input == null) ? "" : _input;
        }


        private void Start()
        {
            _client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut);
            _reader = new StreamReader(_client);
            _writer = new StreamWriter(_client);

            try
            {
                _client.Connect(_timeout);
            }
            catch
            {
                string errorMessage = $"Pipe Client: Connection Timeout";
                _errorBuffer.Enqueue(errorMessage);

                if (_errorToConsole)
                {
                    Console.WriteLine("Pipe Client: Connection Timeout");
                }
                return;
            }

            bool enable = true;
            bool continousRun = false;
            while (enable)
            {
                try
                {
                    if (_client.IsConnected)
                    {
                        _writer.WriteLine(_output);
                        _writer.Flush();
                        _output = "";
                        _input = _reader.ReadLine();
                    }
                    enable = continousRun;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Pipe Client: {ex.Message}";
                    _errorBuffer.Enqueue(errorMessage);

                    if (_errorToConsole)
                    {
                        Console.WriteLine($"Pipe Client: {ex.Message}");
                    }
                }
            }

            _client.Close();
            _client.Dispose();
        }
    }


    public class FixedSizedQueue<T>
    {
        readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public int Size { get; private set; }

        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public void Enqueue(T obj)
        {
            // removed because of git
            return;
            queue.Enqueue(obj);

            while (queue.Count > Size)
            {
                T outObj;
                queue.TryDequeue(out outObj);
            }
        }
    }
}