using System;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using CoopChaos.Simulation.Factories;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonRoomState))]
    public class ServerCannonRoom : ServerInteractableObjectBase
    {
        private const float BulletLoadSpeed = 0.5f;
        
        private SimulationBehaviour simulationBehaviour;
        private CannonRoomState state;

        public override void Interact(ulong clientId)
        {
            base.Interact(clientId);
        }

        public void Shoot()
        {
            if (state.BulletLoad.Value < 1f)
            {
                Debug.LogWarning("User tried to shoot but no bullets");
            }

            var spaceship = simulationBehaviour.World.PlayerSpaceship.Value;
            ref var spaceshipObject = ref spaceship.Get<ObjectComponent>();

            simulationBehaviour.World.CreateProjectile(
                ref spaceshipObject, 70f, state.Angle.Value, 1000f, 1000f);

            state.BulletLoad.Value = 0f;
        }

        private void Awake()
        {
            state = GetComponent<CannonRoomState>();
            simulationBehaviour = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(state);
            Assert.IsNotNull(simulationBehaviour);
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (state.BulletLoad.Value < 1f)
            {
                state.BulletLoad.Value =
                    Mathf.MoveTowards(state.BulletLoad.Value, 1f, BulletLoadSpeed * Time.deltaTime);
            }
        }
    }
}