using CoopChaos.Simulation.Components;
using DefaultEcs;

namespace CoopChaos.Simulation.Systems
{
    public class ObjectMovementSystem : ISystem
    {
        private World world;
        private EntitySet objects;
        
        public World World { get; set; }

        public ObjectMovementSystem(World world)
        {
            this.world = world;

            objects = World.GetEntities()
                .With<ObjectComponent>()
                .AsSet();
        }

        public void Update(float deltaTime)
        {
            foreach (var entity in objects.GetEntities())
            {
                ref var oc = ref entity.Get<ObjectComponent>();
                
                oc.X += oc.VelocityX * deltaTime;
                oc.Y += oc.VelocityY * deltaTime;
                
                if (oc.VelocityX != 0f || oc.VelocityY != 0f)
                {
                    entity.NotifyChanged<ObjectComponent>();
                }
            }
        }
    }
}