using Features.Data.Components;
using Features.Models;
using Unity.Entities;
using Unity.Mathematics;
using Zenject;

namespace Features.BoardStateChart.BoardStates
{
    public class FallingState : BoardBaseState
    {
        [Inject] 
        private EntityManager entityManager;
        
        [Inject] 
        private BoardModel boardModel;
        
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