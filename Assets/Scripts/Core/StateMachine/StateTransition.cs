using System;

namespace Core.StateMachine
{
    public class StateTransition
    {
        public StateVertex SourceStateVertex;
        public StateVertex TargetStateVertex;
        public IStateMachineEvent TriggerEvent;
        
        public StateTransition(StateVertex source)
        {
            SourceStateVertex = source;
        }
        
        public StateTransition Target(StateVertex target)
        {
            if (TargetStateVertex != null) throw new Exception("Duplicated transition target");
            TargetStateVertex = target;
            TargetStateVertex.AddIncomingTransition(this);
            return this;
        }


        public bool ExecuteTrigger(StateChart stateChart, IStateMachineEvent stateMachineEvent)
        {
            if (TriggerEvent == stateMachineEvent)
            {
                stateChart.PrepareTransition(TargetStateVertex);
                return true;
            }

            return false;
        }

        public StateTransition OnEvent(IStateMachineEvent trigger)
        {
            TriggerEvent = trigger;
            return this;
        }
    }
}