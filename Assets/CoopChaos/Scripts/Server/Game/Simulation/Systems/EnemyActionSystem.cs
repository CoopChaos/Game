using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Systems
{
    public class EnemyActionSystem : ISystem
    {
        private EntitySet enemies;
        private CoopChaosWorld world;
        
        public EnemyActionSystem(CoopChaosWorld world)
        {
            enemies = world.Native.GetEntities()
                .With<EnemyComponent>()
                .AsSet();
            
            this.world = world;
        }
        
        public void Update(float deltaTime)
        {
            foreach (var entity in enemies.GetEntities())
            {
                ref var oc = ref entity.Get<EnemyComponent>();

                oc.ActionCooldown = Mathf.MoveTowards(oc.ActionCooldown, 0, deltaTime);
                
                if (oc.ActionCooldown > 0)
                {
                    continue;
                }
                
                float sum = 0;
                
                foreach (var action in oc.Actions)
                {
                    sum += action.ProbabilisticWeight;
                }
                
                var r = Random.Range(0, sum);
                
                foreach (var action in oc.Actions)
                {
                    r -= action.ProbabilisticWeight;
                    
                    if (r <= 0)
                    {
                        action.PerformAction(world, entity);
                        oc.ActionCooldown = action.Frequency;
                        break;
                    }
                }
            }
        }
    }
}