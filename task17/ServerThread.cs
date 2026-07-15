using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public interface ICommand
    {
        void Execute();
    }

    public class ServerThread
    {
        BlockingCollection<ICommand> _queue = new();
        Thread _thread;
        Action _strategy;
        volatile bool _isRunning = true;

        public Thread UnderlyingThread => _thread;

        public ServerThread()
        {
            _thread = new Thread(Run) { IsBackground = true };
            _strategy = DefaultStrategy;
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Enqueue(ICommand command)
        {
            if (!_queue.IsAddingCompleted)
            {
                _queue.Add(command);
            }
        }

        void Run()
        {
            while (_isRunning)
            {
                try
                {
                    _strategy();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в потоке: {ex.Message}");
                }
            }
        }

        void DefaultStrategy()
        {
            try
            {
                ICommand command = _queue.Take();
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в команде {command.GetType().Name}: {ex.Message}");
                }
            }
            catch (InvalidOperationException)
            {
                _isRunning = false;
            }
        }

        void SoftStopStrategy()
        {
            if (_queue.TryTake(out ICommand? command) && command != null)
            {
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в команде {command.GetType().Name}: {ex.Message}");
                }
            }
            else
            {
                _isRunning = false;
            }
        }

        public void HardStop()
        {
            VerifyCurrentThread();
            _isRunning = false;
        }

        public void SoftStop()
        {
            VerifyCurrentThread();
            _strategy = SoftStopStrategy;
        }

        void VerifyCurrentThread()
        {
            if (Thread.CurrentThread != _thread)
            {
                throw new InvalidOperationException("Команда должна выполняться только внутри ServerThread");
            }
        }
    }

    public class HardStopCommand : ICommand
    {
        ServerThread _serverThread;

        public HardStopCommand(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }

        public void Execute()
        {
            _serverThread.HardStop();
        }
    }

    public class SoftStopCommand : ICommand
    {
        ServerThread _serverThread;

        public SoftStopCommand(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }

        public void Execute()
        {
            _serverThread.SoftStop();
        }
    }
}
