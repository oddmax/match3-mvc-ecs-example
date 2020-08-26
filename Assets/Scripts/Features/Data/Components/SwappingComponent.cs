using Unity.Entities;
using Unity.Mathematics;

namespace Features.Data.Components
{
    public struct SwappingComponent : IComponentData
    {
        public int2 OriginBoardPosition;
        public int2 TargetBoardPosition;
    }
}