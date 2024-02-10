namespace R8.TimeMachine
{
    internal class ControllableTimerFactory : IControllableTimerFactory
    {
        public IControllableTimer Create()
        {
            return new ControllableTimer();
        }
    }
}