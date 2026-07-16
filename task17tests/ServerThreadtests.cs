using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using task17;


    public class SpyCommand : ICommand
    {
        public bool WasExecuted { get; private set; }
        int _delay;

        public SpyCommand(int delay = 0)
        {
            _delay = delay;
        }

        public void Execute()
        {
            if (_delay > 0)
            {
                Thread.Sleep(_delay);
            }
            WasExecuted = true;
        }
    }

    public class LongRunningSpyCommand : ICommand
    {
        private readonly IScheduler _scheduler;
        private int _stepsLeft;
        public int StepsExecuted { get; private set; }
        public Action? OnExecute { get; set; }

        public LongRunningSpyCommand(IScheduler scheduler, int totalSteps)
        {
            _scheduler = scheduler;
            _stepsLeft = totalSteps;
        }

        public void Execute()
        {
            StepsExecuted++;
            _stepsLeft--;
            OnExecute?.Invoke();
            if (_stepsLeft > 0)
                _scheduler.Add(this);
        }
    }

    public class ServerThreadTests
    {
        [Fact]
        public void HardStopCommand_ShouldStopThread_IgnoringRemainingCommands()
        {
            var server = new ServerThread();
            var cmd1 = new SpyCommand();
            var hardStop = new HardStopCommand(server);
            var cmd2 = new SpyCommand();
            server.Enqueue(cmd1);
            server.Enqueue(hardStop);
            server.Enqueue(cmd2);
            server.Start();
            server.UnderlyingThread.Join(1000);
            Assert.True(cmd1.WasExecuted);
            Assert.False(cmd2.WasExecuted);
            Assert.False(server.UnderlyingThread.IsAlive);
        }

        [Fact]
        public void SoftStopCommand_ShouldProcessAllQueuedCommands()
        {
            var server = new ServerThread();
            var cmd1 = new SpyCommand();
            var softStop = new SoftStopCommand(server);
            var cmd2 = new SpyCommand();
            server.Enqueue(cmd1);
            server.Enqueue(softStop);
            server.Enqueue(cmd2);
            server.Start();
            server.UnderlyingThread.Join(1000);
            Assert.True(cmd1.WasExecuted);
            Assert.True(cmd2.WasExecuted);
            Assert.False(server.UnderlyingThread.IsAlive);
        }

        [Fact]
        public void Commands_ShouldThrowException_ExecutedInWrongThread()
        {
            var server = new ServerThread();
            server.Start();
            var hardStop = new HardStopCommand(server);
            var softStop = new SoftStopCommand(server);
            Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
            Assert.Throws<InvalidOperationException>(() => softStop.Execute());
            server.Enqueue(new HardStopCommand(server));
            server.UnderlyingThread.Join(500);
        }

        [Fact]
        public void LongRunningCommand_ShouldExecuteAllSteps()
        {
            var server = new ServerThread();
            var cmd = new LongRunningSpyCommand(server.Scheduler, 5);
            server.Enqueue(cmd);
            server.Start();
            WaitFor(() => cmd.StepsExecuted == 5);
            server.Enqueue(new HardStopCommand(server));
            server.UnderlyingThread.Join(1000);
            Assert.Equal(5, cmd.StepsExecuted);
        }

        [Fact]
        public void Scheduler_ShouldUseRoundRobin()
        {
            var server = new ServerThread();
            var log = new List<string>();
            var cmd1 = new LongRunningSpyCommand(server.Scheduler, 3) { OnExecute = () => log.Add("A") };
            var cmd2 = new LongRunningSpyCommand(server.Scheduler, 3) { OnExecute = () => log.Add("B") };
            server.Enqueue(cmd1);
            server.Enqueue(cmd2);
            server.Start();
            WaitFor(() => log.Count == 6);
            server.Enqueue(new HardStopCommand(server));
            server.UnderlyingThread.Join(1000);
            Assert.Equal("ABABAB", string.Join("", log));
        }

        [Fact]
        public void NewCommands_ShouldNotStarve()
        {
            var server = new ServerThread();
            var longCmd = new LongRunningSpyCommand(server.Scheduler, 100);
            var shortCmd = new SpyCommand();
            server.Enqueue(longCmd);
            server.Start();
            Thread.Sleep(50);
            server.Enqueue(shortCmd);
            WaitFor(() => shortCmd.WasExecuted);
            Assert.True(shortCmd.WasExecuted);
            server.Enqueue(new HardStopCommand(server));
            server.UnderlyingThread.Join(1000);
        }

        [Fact]
        public void SoftStop_ShouldDrainScheduler()
        {
            var server = new ServerThread();
            var cmd = new LongRunningSpyCommand(server.Scheduler, 5);
            server.Enqueue(cmd);
            server.Start();
            WaitFor(() => cmd.StepsExecuted >= 2);
            server.Enqueue(new SoftStopCommand(server));
            server.UnderlyingThread.Join(1000);
            Assert.Equal(5, cmd.StepsExecuted);
            Assert.False(server.UnderlyingThread.IsAlive);
        }

        private void WaitFor(Func<bool> condition, int timeoutMs = 3000)
        {
            var start = DateTime.Now;
            while (!condition())
            {
                if ((DateTime.Now - start).TotalMilliseconds > timeoutMs)
                    throw new TimeoutException();
                Thread.Sleep(10);
            }
        }
    }
