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

            destroyedEntities = world.Native.GetEntities()
                .With<ObjectComponent>()
                .WhenAdded<DestroyComponent>()
                .AsSet();
        }
        
        public void Update(float deltaTime)
        {
            foreach (var entity in destroyedEntities.GetEntities())
            {
                if (entity.Has<PlayerSpaceshipComponent>())
                {
                    world.Native.Publish(new PlayerSpaceshipDestroyedEvent()
                    {
                        Entity = entity
                    });
                }
                
                Debug.Log("Destroyed entity: " + entity.Get<DetectionTypeComponent>().Type);

                entity.Dispose();
            }
        }
    }
}