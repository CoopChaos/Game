using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class PlayerSpaceshipVelocitySystem : ISystem
    {
        private const float Speed = 10f;
        
        private CoopChaosWorld world;
        
        public PlayerSpaceshipVelocitySystem(CoopChaosWorld world)
        {
            this.world = world;
        }
        
        public void Update(float deltaTime)
        {
            ref var psc = ref world.PlayerSpaceship.Value.Get<PlayerSpaceshipComponent>();
            ref var oc = ref world.PlayerSpaceship.Value.Get<ObjectComponent>();

            bool changed = false;

            if (psc.TargetHorizontalVelocity != oc.VelocityX)
            {
                oc.VelocityX = Mathf.MoveTowards(oc.VelocityX, psc.TargetHorizontalVelocity, deltaTime * Speed);
                changed = true;
            }
            
            if (psc.TargetVerticalVelocity != oc.VelocityY)
            {
                oc.VelocityY = Mathf.MoveTowards(oc.VelocityY, psc.TargetVerticalVelocity, deltaTime * Speed);
                changed = true;
            }
            
            if (changed)
            {
                world.PlayerSpaceship.Value.NotifyChanged<ObjectComponent>();
            }
        }
    }
}