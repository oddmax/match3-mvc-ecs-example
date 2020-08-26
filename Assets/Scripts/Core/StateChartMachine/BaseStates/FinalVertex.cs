namespace Core.StateChartMachine.BaseStates
{
    public class FinalVertex : StateVertex
    {
        public FinalVertex(IState state) : base(state)
        {
            vertexType = VertexType.Final;
        }
    }
}