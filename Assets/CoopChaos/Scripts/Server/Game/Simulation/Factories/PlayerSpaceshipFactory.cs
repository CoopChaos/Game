using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Events;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Factories
{
    public static class PlayerSpaceshipFactory
    {
        private const float ShipMass = 100f;
        private const float ShipSize = 10f;
        
        public static Entity CreatePlayerSpaceship(this CoopChaosWorld world, float x, float y)
        {
            var entity = world.Native.CreateEntity();
            Debug.Log("Creating spaceship");
            entity.Set(new ObjectComponent()
            {
                Health = 600f,
                X = x,
                Y = y,
                VelocityX = 0,
                VelocityY = 0,
                Mass = ShipMass,
                Size = ShipSize,
            });
            
            entity.Set(new PlayerSpaceshipComponent()
            {
                TargetHorizontalVelocity = 0f,
                TargetVerticalVelocity = 0f,
            });
            
            entity.Set(new DetectionTypeComponent()
            {
                Type = DetectionType.AliveShipObject
            });
            
            world.Native.Publish(new PlayerSpaceshipSpawnEvent()
            {
                Health = entity.Get<ObjectComponent>().Health
            });
            return entity;
        }
    }
}