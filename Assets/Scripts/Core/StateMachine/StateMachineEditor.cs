using Core.StateMachine.BaseStates;

namespace Core.StateMachine
{
    public class StateMachineEditor
    {
        private StateChart stateChart;

        public StateMachineEditor(StateChart stateChart)
        {
            this.stateChart = stateChart;
        }
        
        public StateVertex Initial()
        {
            var initialVertex = new InitialVertex(null);
            stateChart.SetInitialVertex(initialVertex);
            return initialVertex;
        }

        public StateVertex Final()
        {
            var finalVertex = new FinalVertex(null);
            AddVertex(finalVertex);
            return finalVertex;
        }
        
        private void AddVertex(StateVertex vertex)
        {
            stateChart.AddVertex(vertex);
        }
        
        public void Destroy()
        {
            stateChart = null;
        }
    }
}