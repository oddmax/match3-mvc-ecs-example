using System.Collections.Generic;
using Core.StateMachine.BaseStates;

namespace Core.StateMachine
{
    internal class StateEvents
    {
        public static readonly IStateMachineEvent StartEvent = new StateMachineEvent();
    }
    
    /// <summary>
    /// Basic state chart machine to switch between states based on IStateMachineEvent 
    /// </summary>
    public class StateChart
    {
        private StateVertex currentStateVertex;
        private List<StateVertex> Vertices = new List<StateVertex>();
        private InitialVertex startVertex;

        public void Start()
        {
            startVertex.ExecuteTrigger(this, StateEvents.StartEvent);
        }

        public void Stop()
        {
            currentStateVertex?.OnExit();
        }
        
        public void Trigger(IStateMachineEvent trigger)
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

        public void AddVertex(StateVertex vertex)
        {
            Vertices.Add(vertex);
        }

        public void SetInitialVertex(InitialVertex initialVertex)
        {
            this.startVertex = initialVertex;
            Vertices.Add(initialVertex);
        }
    }
}