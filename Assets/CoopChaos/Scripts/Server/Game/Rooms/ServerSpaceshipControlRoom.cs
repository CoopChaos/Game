using System;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(SpaceshipControlRoomState))]
    public class ServerSpaceshipControlRoom : ServerInteractableObjectBase
    {
        private const float VerticalMaxSpeed = 50.0f;
        private const float HorizontalMaxSpeed = 30.0f;
        private SpaceshipControlRoomState interactableState;
        private SimulationBehaviour simulation;
        
        public override void Interact(ulong clientId)
        {
            base.Interact(clientId);
        }

        public void SetVertical(float value)
        {
            value = Mathf.Clamp01(value);
            
            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipComponent = ref spaceship.Value.Get<ObjectComponent>();

            spaceshipComponent.VelocityY = VerticalMaxSpeed * value;
            interactableState.SetVerticalVelocityServerRpc(spaceshipComponent.VelocityY);
        }

        public void SetHorizontal(float value)
        {
            value = Mathf.Clamp01(value);
            
            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipComponent = ref spaceship.Value.Get<ObjectComponent>();

            spaceshipComponent.VelocityX = (float)Math.Round(HorizontalMaxSpeed * (value - 0.5f), 2);
            interactableState.SetHorizontalVelocityServerRpc(spaceshipComponent.VelocityX);
        }

        private void Awake()
        {
            interactableState = GetComponent<SpaceshipControlRoomState>();
            Assert.IsNotNull(interactableState);

            simulation = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(simulation);
        }

        protected override void Start()
        {
            base.Start();
        }

    }
}
