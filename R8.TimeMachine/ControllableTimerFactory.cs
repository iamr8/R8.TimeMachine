namespace R8.TimeMachine
{
    public class ControllableTimerFactory : IControllableTimerFactory
    {
        public IControllableTimer Create()
        {
            return new ControllableTimer();
        }
    }
}