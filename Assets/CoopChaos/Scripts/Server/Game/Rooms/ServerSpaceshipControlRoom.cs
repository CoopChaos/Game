using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(SpaceshipControlRoomState))]
    public class ServerSpaceshipControlRoom : ServerInteractableObjectBase
    {
        private SpaceshipControlRoomState interactableState;
        public override void Interact(ulong clientId)
        {
            
        }
        
        private void Awake()
        {
            interactableState = GetComponent<SpaceshipControlRoomState>();
            Assert.IsNotNull(interactableState);
        }

        protected override void Start()
        {
            base.Start();
        }

    }
}
