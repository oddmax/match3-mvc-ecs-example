using System;

namespace StateChart
{
    public class StateTransition
    {
        public StateVertex SourceStateVertex;
        public StateVertex TargetStateVertex;
        public IStateChartEvent TriggerEvent;
        
        public StateTransition(StateVertex source)
        {
            SourceStateVertex = source;
        }
        
        public StateTransition Target(StateVertex target)
        {
            if (TargetStateVertex != null) throw new Exception("Duplicated transition target");
            TargetStateVertex = (StateVertex)target;
            TargetStateVertex.AddIncomingTransition(this);
            return this;
        }


        public bool ExecuteTrigger(StateChart stateChart, IStateChartEvent stateChartEvent)
        {
            if (TriggerEvent == stateChartEvent)
            {
                stateChart.PrepareTransition(TargetStateVertex);
                return true;
            }

            return false;
        }

        public StateTransition OnEvent(IStateChartEvent trigger)
        {
            TriggerEvent = trigger;
            return this;
        }
    }
}