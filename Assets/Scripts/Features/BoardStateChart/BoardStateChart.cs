using System;
using Core.StateMachine;
using Features.BoardStateChart.BoardStates;
using Features.Data;
using Features.Signals;
using JetBrains.Annotations;
using Zenject;

namespace Features.BoardStateChart
{
    [UsedImplicitly]
    public class BoardStateEvents
    {
        public static readonly IStateMachineEvent StartSwapEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent SwapCompleteEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent RevertSwapEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent NoMatchesFoundEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent MatchesFoundEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent NoTurnsLeftEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent FallingCompleteEvent = new StateMachineEvent();
        public static readonly IStateMachineEvent ExitBoardEvent = new StateMachineEvent();
    }
    
    /// <summary>
    /// State Chart Machine to switch between different states on the board
    /// </summary>
    [UsedImplicitly]
    public class BoardStateChart : IInitializable, IDisposable
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
            
            signalBus.Subscribe<Match3Signals.StateChartSignal>(OnStateChartSignal);
        }

        private void OnStateChartSignal(Match3Signals.StateChartSignal signal)
        {
            stateChart.Trigger(signal._stateMachineEvent);
        }
        
        private void SetupStateChart(StateChart chart)
        {
            var playerTurnState = CreateState<PlayerTurnState>(Match3State.PlayerTurn);
            var swapInProgressState = CreateState<SwapInProgressState>(Match3State.SwapInProgress);
            var revertSwapInProgressState = CreateState<RevertSwapInProgressState>(Match3State.RevertSwapInProgress);
            var matchesSearchState = CreateState<MatchesSearchState>(Match3State.MatchesSearch);
            var fallingState = CreateState<FallingState>(Match3State.MatchesFall);
            var gameOverState = CreateState<GameOverState>(Match3State.GameOver);
            
            var editor = new StateMachineEditor(chart);
            editor.Initial().Transition().Target(playerTurnState);
            
            playerTurnState.Event(BoardStateEvents.StartSwapEvent).Target(swapInProgressState);
            
            swapInProgressState.Event(BoardStateEvents.SwapCompleteEvent).Target(matchesSearchState);
            revertSwapInProgressState.Event(BoardStateEvents.SwapCompleteEvent).Target(playerTurnState);
            
            matchesSearchState.Event(BoardStateEvents.RevertSwapEvent).Target(revertSwapInProgressState);
            matchesSearchState.Event(BoardStateEvents.NoMatchesFoundEvent).Target(playerTurnState);
            matchesSearchState.Event(BoardStateEvents.MatchesFoundEvent).Target(fallingState);
            
            playerTurnState.Event(BoardStateEvents.NoTurnsLeftEvent).Target(gameOverState);
            
            fallingState.Event(BoardStateEvents.FallingCompleteEvent).Target(matchesSearchState);
            
            gameOverState.Event(BoardStateEvents.ExitBoardEvent).Target(editor.Final());
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<Match3Signals.StateChartSignal>(OnStateChartSignal);
            stateChart = null;
        }

        private StateVertex CreateState<T>(Match3State stateEnum) where T:BoardBaseState
        {
            T state = diContainer.Instantiate<T>();
            state.SetState(stateEnum);
            return new StateVertex(state);
        }
    }
}