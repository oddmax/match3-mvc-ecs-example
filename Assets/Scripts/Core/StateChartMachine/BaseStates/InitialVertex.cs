namespace StateChart.BaseStates
{
    public class InitialVertex : StateVertex
    {
        public InitialVertex(IState state) : base(state)
        {
            vertexType = VertexType.Initial;
        }

        public override StateTransition Transition()
        {
            var transition = base.Transition();
            transition.TriggerEvent = StateEvents.StartEvent;
            return transition;
        }
    }
}