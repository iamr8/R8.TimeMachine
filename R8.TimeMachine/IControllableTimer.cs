using System;
using System.Threading;

namespace R8.TimeMachine
{
    public interface IControllableTimer : IDisposable
    {
        /// <summary>
        ///     An action that will be called when the timer is triggered.
        /// </summary>
        TimerCallback? Callback { get; }

        /// <summary>
        ///     Gets a value indicating whether the timer is started.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        ///     Sets the callback for the timer and starts it.
        /// </summary>
        /// <param name="callback">The callback to be called when the timer is triggered.</param>
        /// <param name="autoStart">Whether the timer should be started immediately.</param>
        /// <exception cref="ArgumentNullException">The callback parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the callback was already set.</exception>
        void OnCallback(TimerCallback callback, bool autoStart = true);

        /// <summary>
        ///     Sets the callback for the timer and starts it.
        /// </summary>
        /// <param name="callback">The callback to be called when the timer is triggered.</param>
        /// <param name="state">The state to be passed to the callback.</param>
        /// <param name="autoStart">Whether the timer should be started immediately.</param>
        /// <exception cref="ArgumentNullException">The callback parameter is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the callback was already set.</exception>
        void OnCallback(TimerCallback callback, object state, bool autoStart = true);

        /// <summary>
        ///     Changes the due time of the timer.
        /// </summary>
        /// <param name="dueTime">The new due time.</param>
        /// <exception cref="ObjectDisposedException">The Timer has already been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown the due time is <see cref="Timeout.InfiniteTimeSpan" />.</exception>
        bool Change(TimeSpan dueTime);

        /// <summary>
        ///     Changes the due time of the timer.
        /// </summary>
        /// <param name="dueTime">The new due time.</param>
        /// <param name="period">The new period.</param>
        /// <exception cref="ObjectDisposedException">The Timer has already been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown the due time or period is <see cref="Timeout.InfiniteTimeSpan" />.</exception>
        bool Change(TimeSpan dueTime, TimeSpan period);

        /// <summary>
        ///     Stops the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The Timer has already been disposed.</exception>
        bool Stop();

        /// <summary>
        ///     Starts the timer. If the timer is already started, it will be restarted.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The Timer has already been disposed.</exception>
        /// <exception cref="InvalidOperationException">Thrown the due time or period is <see cref="Timeout.InfiniteTimeSpan" />.</exception>
        /// <exception cref="InvalidOperationException">Thrown the timer is not initialized.</exception>
        bool Start();
    }
}