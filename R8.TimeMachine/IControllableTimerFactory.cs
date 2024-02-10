namespace R8.TimeMachine
{
    public interface IControllableTimerFactory
    {
        /// <summary>
        ///     Creates a new instance of <see cref="IControllableTimer" />.
        /// </summary>
        /// <returns>A new instance of <see cref="IControllableTimer" />.</returns>
        IControllableTimer Create();
    }
}