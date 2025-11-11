using ECS_Framework.ECS;
using ECS_Framework.Components;
using System.Collections.Generic;
using System.Drawing;

namespace ECS_Framework.Systems
{
    public class LifetimeSystem : ISystem
    {
        public void Update(World world, float deltaTime)
        {
            var lifetimePool = world.GetPool<Lifetime>();
            var toRemove = new List<Entity>();

            // Loop Over All Entities With a lifetime component.
            foreach (int entityId in lifetimePool.AllEntityIds())
            {
                var entity = new Entity(entityId);

                // Get the real values of an entites lifetime and update it as the time passes.
                ref var lifetime = ref lifetimePool.Get(entity);
                lifetime.TimeLeft -= deltaTime;

                // If its life is 0 or below schedule it to remove.
                if (lifetime.TimeLeft <= 0f)
                {
                    toRemove.Add(entity);

                }

            }
            
            // Remove all entities that are dead from the lifetime pool WITHOUT
            // altering the enumeration of the original forloop.
            foreach (var entity in toRemove)
            {
                world.DestroyEntity(entity);

            }
                

        }
    }
}