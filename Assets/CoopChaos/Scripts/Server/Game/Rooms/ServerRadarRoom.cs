using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Factories;
using DefaultEcs;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Assertions;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;

namespace CoopChaos.Rooms
{
    [RequireComponent(typeof(RadarState))]
    public class ServerRadarRoom : NetworkBehaviour
    {
        private RadarState radarState;

        private SimulationBehaviour simulation;
        private float lastTime = 0f;

        private EntitySet entities;
        private EntitySet playerSpaceship;


        public override void OnNetworkSpawn()
        {
            if(!IsServer)
            {
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            radarState = GetComponent<RadarState>();
            Assert.IsNotNull(radarState);

            
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

                if (playerSpaceship.TryGetSingleEntity(out var spaceship))
                {
                    // clear radar list
                    radarState.RadarEntities.Clear();
                    ref var spaceshipOc = ref spaceship.Get<ObjectComponent>();
                
                    foreach (var entity in entities.GetEntities())
                    {
                        ref var entityOc = ref entity.Get<ObjectComponent>();
                        ref var entityDetectionType = ref entity.Get<DetectionTypeComponent>();

                        var dx = spaceshipOc.X - entityOc.X;
                        var dy = spaceshipOc.Y - entityOc.Y;
                    
                        var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    
                        if (distance < 10f)
                        {
                            // add to radar list
                            radarState.RadarEntities.Add(new RadarEntity(dx, dy, entityDetectionType.Type));
                            
                            // offset position by spaceship position

                        }
                    }
                }
            }
        }
    }
}