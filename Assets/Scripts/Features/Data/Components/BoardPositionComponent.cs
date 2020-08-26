using Unity.Entities;
using Unity.Mathematics;

namespace Features.Data.Components
{
    public struct BoardPositionComponent : IComponentData
    {
        public int2 Position;
    }
}