namespace Core.StateChartMachine
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }
}