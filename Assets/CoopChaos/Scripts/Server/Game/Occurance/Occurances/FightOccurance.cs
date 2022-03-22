using System.Collections.Generic;
using System.Linq;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Factories;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos
{
    [Occurance(OccuranceType.Fight)]
    public class FightOccurance : IOccurance
    {
        private FightOccuranceDescription description;
        private EntitySet entities;
        
        public FightOccurance(OccuranceDescription description)
        {
            this.description = (FightOccuranceDescription)description;
        }
        
        public string Title => description.Title;
        public string Description => description.Description;
        
        public void Start(SimulationBehaviour simulation)
        {
            ref var oc = ref simulation.World.PlayerSpaceship.Value.Get<ObjectComponent>();

            foreach (var specification in description.EnemySpecifications)
            {
                var enemy = simulation.World.CreateEnemy(new Vector2(oc.X + specification.SpawnXOffset, oc.Y + 400),
                    specification.PatrolPoints.ToList(), specification.Speed, 100f, specification.EnemySize, specification.EnemySize, specification.ShootInterval);
            }

            entities = simulation.World.Native.GetEntities()
                .With<EnemyPatrolComponent>()
                .AsSet();
        }

        public bool Update()
        {            
            // Debug.Log("Emptyness is " + entities.GetEntities().IsEmpty);
            if (entities.GetEntities().IsEmpty == false)
            {
                var oc = entities.GetEntities()[0].Get<ObjectComponent>();
                // Debug.Log("Enemy at " + oc.X + "," + oc.Y);
            }
            

            return !entities.GetEntities().IsEmpty;
        }

        public void Remove()
        {
            foreach (var entity in entities.GetEntities())
            {
                entity.Dispose();
            }
        }
    }
}