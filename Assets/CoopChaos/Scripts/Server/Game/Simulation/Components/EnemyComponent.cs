using System.Collections.Generic;
using CoopChaos.Simulation.Factories;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Simulation.Components
{
    public interface IEnemyAction
    {
        void PerformAction(CoopChaosWorld world, Entity enemy);
        
        float Frequency { get; }
        float ProbabilisticWeight { get; }
    }

    public class SpawnProjectileEnemyAction : IEnemyAction
    {
        public SpawnProjectileEnemyAction(float frequency, float probabilisticWeight)
        {
            Frequency = frequency;
            ProbabilisticWeight = probabilisticWeight;
        }
        
        public float Frequency { get; private set; }
        public float ProbabilisticWeight { get; private set; }

        public void PerformAction(CoopChaosWorld world, Entity enemy)
        {
            ref var oc = ref enemy.Get<ObjectComponent>();
            ref var soc = ref world.PlayerSpaceship.Value.Get<ObjectComponent>();
            
            var source = new Vector2(oc.X, oc.Y);
            var target = new Vector2(soc.X, soc.Y);
            
            var angle = Mathf.Atan2(target.y - source.y, target.x - source.x);
            var angleDegrees = (360 - angle * Mathf.Rad2Deg) + 90;

            world.CreateProjectile(
                ref oc,
                120f,
                angleDegrees,
                100f,
                1000f);
        }
    }
    
    public class EnemyComponent
    {
        public List<IEnemyAction> Actions { get; set; }
        public float ActionCooldown { get; set; }
    }
}