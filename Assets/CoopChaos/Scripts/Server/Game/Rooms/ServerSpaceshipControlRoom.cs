using System;
using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using DefaultEcs;
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
        private SpaceshipControlRoomState state;
        private SimulationBehaviour simulation;
        private EntitySet spaceshipVelocityChanged;
        
        public override void Interact(ulong clientId)
        {
            base.Interact(clientId);
        }

        public void SetVertical(float value)
        {
            value = Mathf.Clamp01(value);
            
            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipComponent = ref spaceship.Value.Get<PlayerSpaceshipComponent>();

            spaceshipComponent.TargetVerticalVelocity = VerticalMaxSpeed * value + 10;
        }

        public void SetHorizontal(float value)
        {
            value = Mathf.Clamp01(value);
            
            var spaceship = simulation.World.PlayerSpaceship;
            ref var spaceshipComponent = ref spaceship.Value.Get<PlayerSpaceshipComponent>();

            spaceshipComponent.TargetHorizontalVelocity = (float) Math.Round(HorizontalMaxSpeed * (value - 0.5f), 2);
        }

        private void Awake()
        {
            state = GetComponent<SpaceshipControlRoomState>();
            Assert.IsNotNull(state);

            simulation = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(simulation);

            spaceshipVelocityChanged = simulation.World.Native.GetEntities()
                .With<PlayerSpaceshipComponent>()
                .WhenChanged<ObjectComponent>()
                .AsSet();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            foreach (var entity in spaceshipVelocityChanged.GetEntities())
            {
                var oc = entity.Get<ObjectComponent>();
                
                state.HorizontalVelocity.Value = oc.VelocityX;
                state.VerticalVelocity.Value = oc.VelocityY;
            }
        }
    }
}
