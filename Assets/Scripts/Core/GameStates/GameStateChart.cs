using System;
using Core.GameStates.States;
using Core.StateMachine;
using Features.Signals;
using JetBrains.Annotations;
using Zenject;

namespace Core.GameStates
{
    [UsedImplicitly]
    public class GameStateEvents
    {
        public static readonly IStateMachineEvent StartLevelEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent ExitBoardEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent DisposedEvent = new StateMachineEvent();
    }
    
    /// <summary>
    /// Main game state chart with basic states - Boot, LevelMap and Board
    /// </summary>
    [UsedImplicitly]
    public class GameStateChart : IInitializable, IDisposable
    {
        [Inject] 
        private DiContainer diContainer;
        
        [Inject] 
        private SignalBus signalBus;

        StateChart stateChart;
        
        public void Initialize()
        {
            stateChart = new StateChart();
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

        private void SetupStateChart(StateChart chart)
        {
            var bootState = new StateVertex(diContainer.Instantiate<BootState>());
            var mapState = new StateVertex(diContainer.Instantiate<LevelSelectionState>());
            var boardState = new StateVertex(diContainer.Instantiate<BoardState>());
            
            var editor = new StateMachineEditor(chart);
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