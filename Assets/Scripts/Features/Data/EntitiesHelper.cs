using System.Collections.Generic;
using Features.Data.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace Features.Data
{
    /// <summary>
    /// Helper class to encapsulate creation of entities
    /// </summary>
    public class EntitiesHelper
    {
        private static EntityManager EntityManager;
        private static EntityArchetype GemArchetype;
        private static Random Random;

        public static void Initialize(EntityManager entityManager)
        {
            EntityManager = entityManager;

            GemArchetype = entityManager.CreateArchetype(
                typeof(LocalTransform),
                typeof(BoardPositionComponent),
                typeof(GemComponent));
            
            Random = new Random();
            Random.InitState();
        }
        
        public static Entity CreateGem(GemColor gemColor, float3 originPosition, int2 boardPosition)
        {
            var entity = EntityManager.CreateEntity(GemArchetype);
            EntityManager.SetComponentData(entity, new LocalTransform {Position = new float3(originPosition.x, originPosition.y, 0)});
            EntityManager.SetComponentData(entity, new BoardPositionComponent {Position = boardPosition});
            EntityManager.SetComponentData(entity, new GemComponent {Color = gemColor});

            return entity;
        }

        public static GemColor GetRandomColor(List<GemColor> availableTypes)
        {
            var nextInt = Random.NextInt(availableTypes.Count);
            return availableTypes[nextInt];
        }
    }
}