using System.Collections;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using DefaultEcs;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ServerRadarRoom : ServerInteractableObjectBase
    {
        private RadarRoomState radarState;
        private SimulationBehaviour simulation;
        private EntitySet entities;

        private Coroutine radarScanCoroutine;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            entities = simulation.World.Native
                .GetEntities()
                .With<ObjectComponent>()
                .With<DetectionTypeComponent>()
                .AsSet();

            radarScanCoroutine = StartCoroutine(RadarScanCoroutine());
        }

        public override void OnNetworkDespawn()
        {
            if (radarScanCoroutine != null)
                StopCoroutine(radarScanCoroutine);
        }

        protected override void Start()
        {
            base.Start();
            Debug.Log("ServerRadarRoom.Start");
        }

        private void Awake()
        {
            Debug.Log("ServerRadarRoom.Awake");
            radarState = GetComponent<RadarRoomState>();
            Assert.IsNotNull(radarState);

            simulation = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(simulation);
        }

        private IEnumerator RadarScanCoroutine()
        {
            while (true)
            {
                UpdateRadarEntities();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void UpdateRadarEntities()
        {
            radarState.RadarEntities.Clear();

            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipObject = ref spaceship.Value.Get<ObjectComponent>();

            radarState.CenterX.Value = spaceshipObject.X;
            radarState.CenterY.Value = spaceshipObject.Y;

            foreach (var entity in entities.GetEntities())
            {
                ref var entityObject = ref entity.Get<ObjectComponent>();

                var dx = spaceshipObject.X - entityObject.X;
                var dy = spaceshipObject.Y - entityObject.Y;

                var distance = Mathf.Sqrt(dx * dx + dy * dy);

                if (radarState.RadarMaxRange > (distance + entityObject.Size))
                {
                    ref var entityDetectionType = ref entity.Get<DetectionTypeComponent>();
                    radarState.RadarEntities.Add(new RadarEntity(entityObject.X, entityObject.Y, entityDetectionType.Type, entityObject.Size));
                }
            }
        }
        
        public override void Interact(ulong clientId)
        {
            base.Interact(clientId);
        }
    }
}