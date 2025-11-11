// The struct of an entity (An ID).
using System;

namespace ECS_Framework.ECS
{
    public readonly struct Entity
    {
        // readonly - Prevents value from being changed
        public readonly int Id;
        public Entity(int id)
        {
            Id = id;
        }
    }
}