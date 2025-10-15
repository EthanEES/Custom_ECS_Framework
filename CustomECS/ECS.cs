namespace CustomECS.ECS
{
    // IComponent - Lets the ECS know which types are valid components
    public interface IComponent { }

    // ISystem - Ensures that all systems have an Update method and updated in a consistent way
    public interface ISystem
    {
        void Update(World world, float deltaTime);
    }


}
