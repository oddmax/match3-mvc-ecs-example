using Unity.Entities;
using Unity.Mathematics;

namespace Data.Components
{
    public struct MoveToBoardPositionComponent : IComponentData
    {
        public int2 TargetPosition;
    }
}