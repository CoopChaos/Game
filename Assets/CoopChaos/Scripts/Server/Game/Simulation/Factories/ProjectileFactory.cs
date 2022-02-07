using CoopChaos.Simulation.Components;
using DefaultEcs;

namespace CoopChaos.Simulation.Factories
{
    public static class ProjectileFactory
    {
        private const float DefaultSize = 0.1f;
        private const float DefaultMass = 1f;
        public static Entity CreateProjectile(this CoopChaosWorld world, ref ObjectComponent source, float velocity, float angle, float damage, float range)
        {
            var entity = world.Native.CreateEntity();

            entity.Set(new ObjectComponent()
            {
                X = 0,
                Y = 0,
                VelocityX = 0,
                VelocityY = 0,
                Mass = DefaultMass,
                Size = DefaultSize,
            });

            entity.Set(new DetectionTypeComponent()
            {
                Type = DetectionType.NaturalDeadObject
            });

            entity.Set(new ProjectileComponent()
            {
                Damage = damage,
                Range = range,
                
            });

            return entity;
        }
    }
}