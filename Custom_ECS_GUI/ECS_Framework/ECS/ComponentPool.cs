// A class to manage all components of a specific type for entities. (I.e ALL <positions> of all entities
// maps the positional data to the entiry based on the Id given.)
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ECS_Framework.ECS
{
    public class ComponentPool<T> where T : struct, IComponent
    {
        // Stores all components of type T, mapped to the entity ID.
        private readonly Dictionary<int, T> _components = new();

        // Adds or updates the component for the given entity.
        public void Add(Entity e, T component)
        {
            _components[e.Id] = component;
        }

        // Removes the component for the given entity.
        public bool Remove(Entity e)
        {
            return _components.Remove(e.Id);
        }

        // Returns a reference to the actual component data for the entity
        public ref T Get(Entity e)
        {
            return ref CollectionsMarshal.GetValueRefOrAddDefault(_components, e.Id, out _);
        }

        // Checks if the entity has a component of this type.
        public bool Has(Entity e)
        {
            return _components.ContainsKey(e.Id);
        }

        // Returns all component values currently stored in the pool
        public IEnumerable<T> AllComponents()
        {
            return _components.Values;
        }

        // Returns all entity IDs that have this component.
        public IEnumerable<int> AllEntityIds()
        {
            return _components.Keys;
        }
    }
}