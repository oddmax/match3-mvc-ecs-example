using Unity.Entities;
using Unity.Transforms;

namespace Features.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class Match3SimulationSystemGroup : ComponentSystemGroup
    {
        
    }
}