using System.Collections.Generic;
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
        private OccuranceDescription description;
        private EntitySet entities;
        
        public FightOccurance(OccuranceDescription description)
        {
            this.description = description;
        }
        
        public string Title => description.Title;
        public string Description => description.Description;
        
        public void Start(SimulationBehaviour simulation)
        {
            var patrolPoints = new List<Vector2>
            {
                new Vector2(-40, 100),
                new Vector2(40, 100)
            };

            ref var oc = ref simulation.World.PlayerSpaceship.Value.Get<ObjectComponent>();

            var enemy = simulation.World.CreateEnemy(new Vector2(oc.X, oc.Y + 400),
                patrolPoints, 40f, 100f, 1f, 10f);

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