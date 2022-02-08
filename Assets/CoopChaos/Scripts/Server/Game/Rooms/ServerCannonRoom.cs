using CoopChaos.Simulation;
using CoopChaos.Simulation.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonRoomState))]
    public class ServerCannonRoom : ServerInteractableObjectBase
    {
        private SimulationBehaviour simulationBehaviour;
        private CannonRoomState interactableState;

        public override void Interact(ulong clientId)
        {
            base.Interact(clientId);
        }
        
        public void Shoot()
        {
            var spaceship = simulationBehaviour.World.PlayerSpaceship.Value;
            ref var spaceshipObject = ref spaceship.Get<ObjectComponent>(); 
            //simulationBehaviour.World.CreateProjectile(ref spaceshipObject, 0, 0, 10, 100);
            
            ref var playerSpaceship = ref spaceship.Get<PlayerSpaceshipComponent>();
            //spaceshipObject.
        }

        private void Awake()
        {
            interactableState = GetComponent<CannonRoomState>();
            simulationBehaviour = FindObjectOfType<SimulationBehaviour>();
            Assert.IsNotNull(interactableState);
            Assert.IsNotNull(simulationBehaviour);
        }

        protected override void Start()
        {
            base.Start();
        }

    }
}
