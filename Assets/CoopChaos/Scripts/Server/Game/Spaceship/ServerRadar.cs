using System;
using System.Collections;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using CoopChaos.Shared;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using DefaultEcs;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ServerRadar : NetworkBehaviour
    {
        private RadarState radarState;
        private SimulationBehaviour simulation;
        private EntitySet entities;

        private Coroutine radarScanCoroutine;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            radarScanCoroutine = StartCoroutine(RadarScanCoroutine());
        }

        public override void OnNetworkDespawn()
        {
            if (radarScanCoroutine != null)
                StopCoroutine(radarScanCoroutine);
        }

        private void Awake()
        {
            radarState = FindObjectOfType<RadarState>();
            Assert.IsNotNull(radarState);

            simulation = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(simulation);

            entities = simulation.World.Native
                .GetEntities()
                .With<ObjectComponent>()
                .With<DetectionTypeComponent>()
                .AsSet();
        }

        private IEnumerator RadarScanCoroutine()
        {
            while (true)
            {
                UpdateRadarEntities();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void UpdateRadarEntities()
        {
            radarState.RadarEntities.Clear();

            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipObject = ref spaceship.Value.Get<ObjectComponent>();

            int i = 0;
            int j = 0;
            
            foreach (var entity in entities.GetEntities())
            {
                ref var entityObject = ref entity.Get<ObjectComponent>();

                var dx = spaceshipObject.X - entityObject.X;
                var dy = spaceshipObject.Y - entityObject.Y;

                var distance = Mathf.Sqrt(dx * dx + dy * dy);

                if (radarState.RadarMaxRange > distance)
                {
                    ++i;
                    ref var entityDetectionType = ref entity.Get<DetectionTypeComponent>();
                    radarState.RadarEntities.Add(new RadarEntity(entityObject.X, entityObject.Y, entityDetectionType.Type));
                }
                else
                {
                    ++j;
                }
            }
            
            Debug.Log($"++{i} sync --{j}");
        }
    }
}
