using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class PlayerSpaceshipMovementSystem : ISystem
    {
        public World World { get; set; }

        public void Update(float deltaTime)
        {
            foreach (var entity in World.GetEntities()
                         .With<PlayerSpaceshipComponent>()
                         .AsEnumerable())
            {
                ref var sc = ref entity.Get<PlayerSpaceshipComponent>();
                ref var oc = ref entity.Get<ObjectComponent>();
                
                oc.VelocityY += deltaTime * sc.VerticalAcceleration;

                if (oc.VelocityY < 0f)
                {
                    oc.VelocityY = 0f;
                }

                if (sc.VerticalAcceleration != 0f)
                {
                    entity.NotifyChanged<ObjectComponent>();
                }
            }
        }
    }
}