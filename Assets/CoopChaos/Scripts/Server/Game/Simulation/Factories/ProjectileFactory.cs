using System;
using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Factories
{
    public static class ProjectileFactory
    {
        private const float Offset = 10f;
        private const float DefaultSize = 2f;
        private const float DefaultMass = 1f;
        
        public static Entity CreateProjectile(this CoopChaosWorld world, ref ObjectComponent source, float velocity, float angle, float damage, float range)
        {
            var entity = world.Native.CreateEntity();
            
            // convert angle to radians
            var radians = angle * Mathf.Deg2Rad;
            
            // convert angle to a normalized vector
            var direction = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
            var length = source.Size + DefaultSize + Offset;

            entity.Set(new ObjectComponent()
            {
                X = source.X + direction.x * length,
                Y = source.Y + direction.y * length,
                VelocityX = direction.x * velocity,
                VelocityY = direction.y * velocity,
                Mass = DefaultMass,
                Size = DefaultSize,
            });

            entity.Set(new DetectionTypeComponent()
            {
                Type = DetectionType.AliveProjectileObject
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