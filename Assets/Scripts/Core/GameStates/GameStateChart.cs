using System;
using Signals;
using StateChart;
using Zenject;

namespace DefaultNamespace.States
{
    public class GameStateEvents
    {
        public static readonly IStateChartEvent StartLevelEvent = new StateChartEvent();
        public static readonly IStateChartEvent ExitBoardEvent = new StateChartEvent();
        public static readonly IStateChartEvent DisposedEvent = new StateChartEvent();
    }
    
    /// <summary>
    /// Main game state chart with basic states - Boot, LevelMap and Board
    /// </summary>
    public class GameStateChart : IInitializable, IDisposable
    {
        [Inject] 
        private DiContainer diContainer;
        
        [Inject] 
        private SignalBus signalBus;

        StateChart.StateChart stateChart;
        
        public void Initialize()
        {
            stateChart = new StateChart.StateChart();
            SetupStateChart(stateChart);
            stateChart.Start();
            
            signalBus.Subscribe<ChangeLevelSignal>(OnLevelChange);
            signalBus.Subscribe<ExitToMapSignal>(OnExitToMap);
        }

        private void OnExitToMap()
        {
            stateChart.Trigger(GameStateEvents.ExitBoardEvent);
        }

        private void OnLevelChange()
        {
            stateChart.Trigger(GameStateEvents.StartLevelEvent);
        }

        private void SetupStateChart(StateChart.StateChart chart)
        {
            var bootState = new StateVertex(diContainer.Instantiate<BootState>());
            var mapState = new StateVertex(diContainer.Instantiate<MapState>());
            var boardState = new StateVertex(diContainer.Instantiate<BoardState>());
            
            var editor = new StateChartEditor(chart);
            editor.Initial().Transition().Target(mapState);
            bootState.Transition().Target(mapState);
            mapState.Event(GameStateEvents.StartLevelEvent).Target(boardState);
            boardState.Event(GameStateEvents.ExitBoardEvent).Target(mapState);
            mapState.Event(GameStateEvents.DisposedEvent).Target(editor.Final());
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<ChangeLevelSignal>(OnLevelChange);
            signalBus.Unsubscribe<ExitToMapSignal>(OnExitToMap);
        }
    }
}