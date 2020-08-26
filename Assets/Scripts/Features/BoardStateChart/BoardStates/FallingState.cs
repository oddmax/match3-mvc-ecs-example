using Data;
using Data.Components;
using Models;
using StateChart;
using Unity.Entities;
using Unity.Mathematics;
using Utils;
using Zenject;

namespace DefaultNamespace.States.BoardStates
{
    public class FallingState : BoardBaseState
    {
        [Inject] 
        private EntityManager entityManager;
        
        [Inject] 
        private CoroutineProvider coroutineProvider; 
        
        [Inject] 
        private SignalBus signalBus;
        
        [Inject] 
        private BoardModel boardModel;
        
        [Inject] 
        private GameStateModel gameStateModel;
        
        public override void OnEnter()
        {
            base.OnEnter();
            for (var x = 0; x < boardModel.BoardWidth; x++)
            {
                for (var y = 0; y < boardModel.BoardHeight; y++)
                {
                    if (!boardModel.HasEntityAt(new int2(x, y))) 
                        continue;
                        
                    var entity = boardModel.GetEntityAt(x, y);
                    entityManager.AddComponent<IsFallingComponent>(entity);
                }
            }
        }
    }
}