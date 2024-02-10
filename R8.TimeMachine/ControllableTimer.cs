using System;
using System.Threading;

namespace R8.TimeMachine
{
    internal class ControllableTimer : IControllableTimer
    {
        private readonly object _lock = new object();
        private ManualResetEventSlim _callbackCompleteEvent = new ManualResetEventSlim(true); // initially set
        private bool _disposed;

        private TimeSpan _dueTime = Timeout.InfiniteTimeSpan;
        private volatile bool _isStarted;
        private TimeSpan _period = Timeout.InfiniteTimeSpan;
        private Timer? _timer;

        public TimerCallback? Callback { get; private set; }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            lock (_lock)
            {
                ThrowIfDisposed();

                if (Timeout.InfiniteTimeSpan == dueTime || Timeout.InfiniteTimeSpan == period)
                    throw new InvalidOperationException("Timer with infinite due time or period cannot be started.");

                if (_timer == null)
                    return false;

                _dueTime = dueTime;
                _period = period;
                return InternalChange();
            }
        }

        public bool Change(TimeSpan dueTime)
        {
            lock (_lock)
            {
                ThrowIfDisposed();

                if (Timeout.InfiniteTimeSpan == dueTime)
                    throw new InvalidOperationException("Timer with infinite due time cannot be started.");

                if (_timer == null)
                    return false;

                _dueTime = dueTime;
                _period = dueTime;
                return InternalChange();
            }
        }

        public void OnCallback(TimerCallback callback, bool autoStart = true)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            lock (_lock)
            {
                ThrowIfDisposed();

                _timer?.Dispose(); // Dispose of old timer if present

                _isStarted = autoStart;
                Callback = state =>
                {
                    _callbackCompleteEvent.Reset(); // reset at the start of callback
                    try
                    {
                        callback(state);
                    }
                    finally
                    {
                        _callbackCompleteEvent.Set(); // set at the end of callback
                    }
                };
                _timer = autoStart
                    ? new Timer(Callback)
                    : new Timer(Callback, null, Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void OnCallback(TimerCallback callback, object state, bool autoStart = true)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            lock (_lock)
            {
                ThrowIfDisposed();

                _timer?.Dispose(); // Dispose of old timer if present

                _isStarted = autoStart;
                Callback = s =>
                {
                    _callbackCompleteEvent.Reset(); // reset at the start of callback
                    try
                    {
                        callback(s);
                    }
                    finally
                    {
                        _callbackCompleteEvent.Set(); // set at the end of callback
                    }
                };
                _timer = autoStart
                    ? new Timer(Callback, state, _dueTime, _period)
                    : new Timer(Callback, state, Timeout.Infinite, Timeout.Infinite);
            }
        }

        public bool Start()
        {
            lock (_lock)
            {
                ThrowIfDisposed();

                if (Timeout.InfiniteTimeSpan == _dueTime || Timeout.InfiniteTimeSpan == _period)
                    throw new InvalidOperationException("Timer with infinite due time or period cannot be started.");

                if (_timer == null)
                    throw new InvalidOperationException("Timer is not initialized. Use OnCallback() to initialize the timer.");

                _isStarted = true;
                return _timer.Change(_dueTime, _period);
            }
        }

        public bool IsStarted => _isStarted;

        public bool Stop()
        {
            lock (_lock)
            {
                ThrowIfDisposed();

                if (_timer == null)
                    return false;

                _isStarted = false;
                return _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed)
                    return;

                _callbackCompleteEvent.Wait(); // Wait for the callback to complete

                _dueTime = Timeout.InfiniteTimeSpan;
                _period = Timeout.InfiniteTimeSpan;
                _timer?.Change(_dueTime, _period);
                _timer?.Dispose();
                _timer = null;
                _callbackCompleteEvent.Dispose();
                _callbackCompleteEvent = null!;
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }

        private bool InternalChange()
        {
            if (_isStarted) return _timer!.Change(_dueTime, _period);

            return true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ControllableTimer));
        }
    }
}