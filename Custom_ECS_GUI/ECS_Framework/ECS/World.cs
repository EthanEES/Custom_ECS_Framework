//  The class that manages entities, systems, and component pools.
using ECS_Framework.Components;
using System;
using System.Collections.Generic;

namespace ECS_Framework.ECS
{
    // World keeps track of all entities, systems, and component pools.
    public class World
    {
        private int _nextEntityId;
        private readonly List<ISystem> _systems = new();
        private readonly Dictionary<Type, object> _componentPools = new();
        private Stack<int> freeEntities = new Stack<int>();
       
        public int TotalEntityCount => _nextEntityId; 
        public int FreeEntityCount => freeEntities.Count;
        public int ActiveEntityCount => _nextEntityId - freeEntities.Count;
        public int DestroyedEntityCount;


        // CreateEntity() gives you a new entity with a unique ID.
        public Entity CreateEntity()
        {
            if (freeEntities.Count > 0)
            {
                return new Entity(freeEntities.Pop());
            }
            else
            {
                return new(_nextEntityId++);    
            }
        }

        // AddSystem() lets you create a system (like movement, rendering).
        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }


        // Update() calls every system's Update method in the world each frame.
        public void Update(float deltaTime)
        {
            foreach (var sys in _systems)
            {
                sys.Update(this, deltaTime);
            }
        }

        public void DestroyEntity(Entity entity)
        {
            freeEntities.Push(entity.Id);
            
            GetPool<Position>().Remove(entity);
            GetPool<Velocity>().Remove(entity);
            GetPool<Lifetime>().Remove(entity);
            GetPool<Size>().Remove(entity);

            DestroyedEntityCount++;

            
        }

        // Get a specific Component Pool of type T.
        public ComponentPool<T> GetPool<T>() where T : struct, IComponent {
            var type = typeof(T);
            if (!_componentPools.ContainsKey(type))
                _componentPools[type] = new ComponentPool<T>();
                
            return (ComponentPool<T>)_componentPools[type];
        }

    }
}