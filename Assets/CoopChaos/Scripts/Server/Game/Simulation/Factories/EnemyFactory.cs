using System.Collections.Generic;
using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Factories
{
    public static class EnemyFactory
    {
        public static Entity CreateEnemy(this CoopChaosWorld world, Vector2 spawn, List<Vector2> patrolPoints, float speed, float health, float mass, float size)
        {
            var entity = world.Native.CreateEntity();
            
            entity.Set(new ObjectComponent()
            {
                Health = health,
                Mass = mass,
                Size = size,
                VelocityX = 0f,
                VelocityY = 0f,
                X = spawn.x,
                Y = spawn.y
            });
            
            entity.Set(new EnemyComponent()
            {
                Actions = new List<IEnemyAction>{ new SpawnProjectileEnemyAction(1, 1) },
                ActionCooldown = 0f
            });

            entity.Set(new EnemyPatrolComponent()
            {
                PatrolPoints = patrolPoints,
                PatrolPosition = spawn,
                PatrolPointIndex = 0,
                PatrolSpeed = speed
            });
            
            entity.Set(new DetectionTypeComponent()
            {
                Type = DetectionType.AliveShipObject
            });
            
            return entity;
        }
    }
}