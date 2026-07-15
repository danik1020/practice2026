using System;
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
    }
