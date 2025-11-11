using ECS_Framework.ECS;
using ECS_Framework.Components;
// Creating a system to render the particles in the console to display they work.

namespace ECS_Framework.Systems
{
    // Define the render system class, implementing an ISystem interface
    public class RenderSystem : ISystem
    {
        public void Update(World world, float deltaTime)
        {
            // Get the pool of all entities that have a Posision components.
            var posPool = world.GetPool<Position>();
            var lifetimePool = world.GetPool<Lifetime>();



            // Loop Over All Entities With Position
            foreach (var entityId in posPool.AllEntityIds())
            {
                var entity = new Entity(entityId);
                var pos = posPool.Get(entity);
                var time = lifetimePool.Get(entity);

            }

        }
    }
}