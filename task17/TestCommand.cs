using System;

namespace task17
{
    public class TestCommand : ICommand
    {
        private readonly int _id;
        private readonly IScheduler _scheduler;
        private readonly ServerThread _server;
        private int _counter = 0;

        public TestCommand(int id, IScheduler scheduler, ServerThread server)
        {
            _id = id;
            _scheduler = scheduler;
            _server = server;
        }

        public void Execute()
        {
            _counter++;
            Console.WriteLine($"Поток {_id} вызов {_counter}");
            
            if (_counter < 3)
            {
                _scheduler.Add(this);
            }
            else if (_id == 5)
            {
                _scheduler.Add(new HardStopCommand(_server));
            }
        }
    }
}