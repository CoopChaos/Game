using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class EnemyPatrolSystem : ISystem
    {
        private EntitySet enemies;
        private CoopChaosWorld world;
        
        public EnemyPatrolSystem(CoopChaosWorld world)
        {
            enemies = world.Native
                .GetEntities()
                .With<EnemyPatrolComponent>()
                .AsSet();
            this.world = world;
        }
        
        public void Update(float deltaTime)
        {
            var spaceship = world.PlayerSpaceship;

            ref var soc = ref spaceship.Value.Get<ObjectComponent>();
            
            foreach (var entity in enemies.GetEntities())
            {
                ref var oc = ref entity.Get<ObjectComponent>();
                ref var epc = ref entity.Get<EnemyPatrolComponent>();

                epc.PatrolPosition = Vector2.MoveTowards(
                    epc.PatrolPosition,
                    epc.PatrolPoints[epc.PatrolPointIndex], 
                    epc.PatrolSpeed * deltaTime);

                if (Vector2.Distance(epc.PatrolPoints[epc.PatrolPointIndex], epc.PatrolPosition) < 0.1f)
                {
                    epc.PatrolPointIndex = (epc.PatrolPointIndex + 1) % epc.PatrolPoints.Count;
                }

                oc.X = soc.X + epc.PatrolPosition.x;
                oc.Y = soc.Y + epc.PatrolPosition.y;
                
                entity.NotifyChanged<ObjectComponent>();
                entity.NotifyChanged<EnemyPatrolComponent>();
            }
        }
    }
}