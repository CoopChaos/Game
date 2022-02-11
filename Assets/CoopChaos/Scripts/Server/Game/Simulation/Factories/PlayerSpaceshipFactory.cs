using CoopChaos.Simulation.Components;
using DefaultEcs;

namespace CoopChaos.Simulation.Factories
{
    public static class PlayerSpaceshipFactory
    {
        private const float ShipMass = 100f;
        private const float ShipSize = 10f;
        
        public static Entity CreatePlayerSpaceship(this CoopChaosWorld world, float x, float y)
        {
            var entity = world.Native.CreateEntity();

            entity.Set(new ObjectComponent()
            {
                Health = 2500f,
                X = x,
                Y = y,
                VelocityX = 0,
                VelocityY = 0,
                Mass = ShipMass,
                Size = ShipSize,
            });
            
            entity.Set(new PlayerSpaceshipComponent()
            {
                VerticalAcceleration = 0f
            });
            
            entity.Set(new DetectionTypeComponent()
            {
                Type = DetectionType.AliveShipObject
            });
            
            return entity;
        }
    }
}