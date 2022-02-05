using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Events;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class ObjectDestroySystem : ISystem
    {
        private CoopChaosWorld world;
        private EntitySet destroyedEntities;
        
        public ObjectDestroySystem(CoopChaosWorld world)
        {
            this.world = world;

            destroyedEntities = World.GetEntities()
                .With<ObjectComponent>()
                .WhenAdded<DestroyComponent>()
                .AsSet();
        }
        
        public World World { get; set; }
        
        public void Update(float deltaTime)
        {
            foreach (var entity in destroyedEntities.GetEntities())
            {
                if (entity.Has<PlayerSpaceshipComponent>())
                {
                    World.Publish(new PlayerSpaceshipDestroyedEvent()
                    {
                        Entity = entity
                    });
                }

                entity.Dispose();
            }
        }
    }
}