using ECS_Framework.ECS;
using ECS_Framework.Components;

namespace ECS_Framework.Systems
{
    public class MovementSystem : ISystem
    {
        public void Update(World world, float deltaTime)
        {
            // Retrieves the Position and Velocity Component Pool (all <T>Components for all entities)
            // from the world.
            var posPool = world.GetPool<Position>();
            var velPool = world.GetPool<Velocity>();

            // Loop Over All Entities With Position
            foreach (var entityId in posPool.AllEntityIds())
            {
                var entity = new Entity(entityId);
                
                // If a entity has a velocity as well as a position, advance it.
                if (velPool.Has(entity))
                {
                    
                    ref var pos = ref posPool.Get(entity);
                    ref var vel = ref velPool.Get(entity);

                    // Update the REAL values of position and velocity for the entity number "entityId"
                    pos.X += vel.X * deltaTime;
                    pos.Y += vel.Y * deltaTime;
                }
            }
        }
    }
}