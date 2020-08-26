using StateChart.BaseStates;

namespace StateChart
{
    public class StateChartEditor
    {
        private StateChart stateChart;

        public StateChartEditor(StateChart stateChart)
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
            stateChart.RegisterVertex(vertex);
        }
        
        public void Destroy()
        {
            stateChart = null;
        }
    }
}