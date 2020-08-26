using System.Collections.Generic;
using Core.StateChartMachine.BaseStates;

namespace Core.StateChartMachine
{
    internal class StateEvents
    {
        public static readonly IStateChartEvent StartEvent = new StateChartEvent();
    }
    
    /// <summary>
    /// Basic state chart machine to switch between states based on events
    /// </summary>
    public class StateChart
    {
        private StateVertex currentStateVertex;
        private List<StateVertex> Vertices = new List<StateVertex>();
        private InitialVertex initialVertex;

        public void Start()
        {
            initialVertex.ExecuteTrigger(this, StateEvents.StartEvent);
        }

        public void Stop()
        {
            currentStateVertex?.OnExit();
        }
        
        public void Trigger(IStateChartEvent trigger)
        {
            currentStateVertex?.ExecuteTrigger(this, trigger);
        }

        public void PrepareTransition(StateVertex targetStateVertex)
        {
            ExecuteTransition(targetStateVertex);
        }

        private void ExecuteTransition(StateVertex targetStateVertex)
        {
            currentStateVertex?.OnExit();

            currentStateVertex = targetStateVertex;
            currentStateVertex?.OnEnter();
        }

        public void RegisterVertex(StateVertex vertex)
        {
            Vertices.Add(vertex);
        }

        public void SetInitialVertex(InitialVertex initialVertex)
        {
            this.initialVertex = initialVertex;
            Vertices.Add(initialVertex);
        }
    }
}