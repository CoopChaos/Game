using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class PlayerSpaceshipMovementSystem : ISystem
    {
        private World world;
        private EntitySet playerSpaceship;
        
        public PlayerSpaceshipMovementSystem(World world)
        {
            this.world = world;
            
            playerSpaceship = world.GetEntities()
                .With<PlayerSpaceshipComponent>()
                .AsSet();
        }

        public void Update(float deltaTime)
        {
            foreach (var entity in playerSpaceship.GetEntities())
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