namespace StateChart
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }
}