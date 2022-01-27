using Unity.Netcode;

namespace CoopChaos
{
    public class SpaceshipControlInteractableState : InteractableObjectStateBase
    {
        private NetworkVariable<bool> isBlocked;

        public NetworkVariable<bool> IsBlocked => isBlocked;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            isBlocked.OnValueChanged += HandleIsBlockedChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            
            isBlocked = new NetworkVariable<bool>(false);
        }
        
        private void HandleIsBlockedChanged(bool blocked, bool oldBlocked)
        {
            // TODO: Show or don't show Spaceship Control
        }
    }
}
