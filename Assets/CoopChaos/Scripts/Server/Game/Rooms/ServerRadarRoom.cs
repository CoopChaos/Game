using System;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;

namespace CoopChaos.Rooms
{
    public class ServerRadarRoom : MonoBehaviour
    {
        private SimulationBehaviour simulation;
        private float lastTime = 0f;

        private EntitySet entities;
        private EntitySet playerSpaceship;

        private void Awake()
        {
            simulation = FindObjectOfType<SimulationBehaviour>();
            
            entities = simulation.World.GetEntities()
                .With<ObjectComponent>()
                .AsSet();
            
            playerSpaceship = simulation.World.GetEntities()
                .With<PlayerSpaceshipComponent>()
                .AsSet();
        }

        private void Update()
        {   
            // run 10 times per second depending on lastTime
            if (Time.time - lastTime > 0.1f)
            {
                lastTime = Time.time;

                var spaceship = playerSpaceship.GetEntities()[0];
                // clear radar list
                ref var spaceshipOc = ref spaceship.Get<ObjectComponent>();
                
                foreach (var entity in entities.GetEntities())
                {
                    ref var entityOc = ref entity.Get<ObjectComponent>();

                    var dx = spaceshipOc.X - entityOc.X;
                    var dy = spaceshipOc.Y - entityOc.Y;
                    
                    var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    
                    if (distance < 10f)
                    {
                        // add to radar list
                        // offset position by spaceship position
                    }
                }
            }
        }
    }
}