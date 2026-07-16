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
        private readonly BlockingCollection<ICommand> _queue = new();
        private readonly IScheduler _scheduler;
        private Thread _thread;
        private Action _strategy;
        private volatile bool _isRunning = true;

        public Thread UnderlyingThread => _thread;
        public IScheduler Scheduler => _scheduler;

        public ServerThread() : this(new RoundRobinScheduler()) { }

        public ServerThread(IScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _thread = new Thread(Run) { IsBackground = true };
            _strategy = DefaultStrategy;
        }

        public void Start() => _thread.Start();

        public void Enqueue(ICommand command)
        {
            if (!_queue.IsAddingCompleted)
                _queue.Add(command);
        }

        private void Run()
        {
            while (_isRunning)
            {
                try { _strategy(); }
                catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
        }

        private void DefaultStrategy()
        {
            if (_queue.TryTake(out ICommand? cmd) && cmd != null)
            {
                ExecuteCommand(cmd);
                return;
            }

            if (_scheduler.HasCommand())
            {
                ExecuteCommand(_scheduler.Select());
                return;
            }

            try
            {
                ExecuteCommand(_queue.Take());
            }
            catch (InvalidOperationException)
            {
                _isRunning = false;
            }
        }

        private void SoftStopStrategy()
        {
            while (true)
            {
                if (_queue.TryTake(out ICommand? cmd) && cmd != null)
                {
                    ExecuteCommand(cmd);
                    continue;
                }
                if (_scheduler.HasCommand())
                {
                    ExecuteCommand(_scheduler.Select());
                    continue;
                }
                break;
            }
            _isRunning = false;
        }

        private void ExecuteCommand(ICommand command)
        {
            try { command.Execute(); }
            catch (Exception ex) { Console.WriteLine($"Ошибка в {command.GetType().Name}: {ex.Message}"); }
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

        private void VerifyCurrentThread()
        {
            if (Thread.CurrentThread != _thread)
                throw new InvalidOperationException("Команда должна выполняться только внутри ServerThread");
        }
    }

    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;
        public HardStopCommand(ServerThread serverThread) => _serverThread = serverThread;
        public void Execute() => _serverThread.HardStop();
    }

    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _serverThread;
        public SoftStopCommand(ServerThread serverThread) => _serverThread = serverThread;
        public void Execute() => _serverThread.SoftStop();
    }
}