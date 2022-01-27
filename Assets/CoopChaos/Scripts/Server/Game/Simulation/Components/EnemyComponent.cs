using System.Collections.Generic;
using DefaultEcs;

namespace CoopChaos.Simulation.Components
{
    public interface IEnemyAction
    {
        void PerformAction(World world, Entity enemy);
        
        float Frequency { get; }
        float ProbabilisticWeight { get; }
    }

    public class SpawnProjectileEnemyAction
    {
    }
    
    public class EnemyComponent
    {
        public List<IEnemyAction> Actions { get; set; }
        public float ActionCooldown { get; set; }
    }
}