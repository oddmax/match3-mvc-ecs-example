namespace Core.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }
}