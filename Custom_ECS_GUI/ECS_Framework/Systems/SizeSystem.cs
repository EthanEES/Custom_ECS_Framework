using ECS_Framework.ECS;
using ECS_Framework.Components;
using System;
using System.Collections.Generic;

namespace ECS_Framework.Systems
{
    public class SizeSystem : ISystem
    {
        private Random rnd = new Random();

        public void Update(World world, float deltaTime)
        {
            // Retrieves the Size Component Pool (all <T>Components for all entities)
            // from the world.
            
            var sizePool = world.GetPool<Size>();
            var toRemove = new List<Entity>();

            // Loop Over All Entities With size
            foreach (int entityId in sizePool.AllEntityIds())
            {
                var entity = new Entity(entityId);
                ref var size = ref sizePool.Get(entity);

                float randomRate = 0.05f + (float)rnd.NextDouble() * 0.10f;
                size.Value -= size.Value * randomRate * deltaTime;

                if(size.Value <= 1)
                {
                    toRemove.Add(entity);
                }
                
            }

            foreach (var entity in toRemove)
            {
                world.DestroyEntity(entity);

            }
        }
    }
}