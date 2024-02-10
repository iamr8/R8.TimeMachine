using System;
using System.Threading;
using NSubstitute;

namespace R8.TimeMachine.Testing
{
    public class FakeControllableTimer : IControllableTimer
    {
        private readonly IControllableTimer _spyTimer = Substitute.For<IControllableTimer>();
        public TimerCallback Callback { get; private set; }
        public bool IsStarted => _spyTimer.IsStarted;

        public bool Change(TimeSpan dueTime)
        {
            return _spyTimer.Change(dueTime);
        }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            return _spyTimer.Change(dueTime, period);
        }

        public void OnCallback(TimerCallback callback, bool autoStart = true)
        {
            _spyTimer.OnCallback(callback, autoStart);
            Callback = callback;
        }

        public void OnCallback(TimerCallback callback, object state, bool autoStart = true)
        {
            _spyTimer.OnCallback(callback, state, autoStart);
            Callback = callback;
        }

        public bool Stop()
        {
            return _spyTimer.Stop();
        }

        public bool Start()
        {
            return _spyTimer.Start();
        }

        public void Dispose()
        {
            _spyTimer.Dispose();
        }
    }
}