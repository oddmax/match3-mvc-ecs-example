using System;
using Data;
using DefaultNamespace.States.BoardStates;
using Signals;
using StateChart;
using Zenject;

namespace DefaultNamespace.States
{
    public class BoardStateEvents
    {
        public static readonly IStateChartEvent StartSwapEvent = new StateChartEvent();
        public static readonly IStateChartEvent SwapCompleteEvent = new StateChartEvent();
        public static readonly IStateChartEvent RevertSwapEvent = new StateChartEvent();
        public static readonly IStateChartEvent NoMatchesFoundEvent = new StateChartEvent();
        public static readonly IStateChartEvent MatchesFoundEvent = new StateChartEvent();
        public static readonly IStateChartEvent NoTurnsLeftEvent = new StateChartEvent();
        public static readonly IStateChartEvent FallingCompleteEvent = new StateChartEvent();
        public static readonly IStateChartEvent ExitBoardEvent = new StateChartEvent();
    }
    
    public class BoardStateChart : IInitializable, IDisposable
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
            
            signalBus.Subscribe<Match3Signals.StateChartSignal>(OnStateChartSignal);
        }

        private void OnStateChartSignal(Match3Signals.StateChartSignal signal)
        {
            stateChart.Trigger(signal.stateChartEvent);
        }
        
        private void SetupStateChart(StateChart.StateChart chart)
        {
            var playerTurnState = CreateState<PlayerTurnState>(Match3State.PlayerTurn);
            var swapInProgressState = CreateState<SwapInProgressState>(Match3State.SwapInProgress);
            var revertSwapInProgressState = CreateState<RevertSwapInProgressState>(Match3State.RevertSwapInProgress);
            var matchesSearchState = CreateState<MatchesSearchState>(Match3State.MatchesSearch);
            var fallingState = CreateState<FallingState>(Match3State.MatchesFall);
            var gameOverState = CreateState<GameOverState>(Match3State.GameOver);
            
            var editor = new StateChartEditor(chart);
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