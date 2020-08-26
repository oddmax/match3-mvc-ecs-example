using Unity.Entities;
using Unity.Mathematics;

namespace Features.Data.Components
{
    public struct MoveToBoardPositionComponent : IComponentData
    {
        public int2 TargetPosition;
    }
}