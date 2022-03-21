using Unity.Netcode;

namespace CoopChaos
{
    public class DoorInteractableState : InteractableObjectStateBase
    {
        private NetworkVariable<bool> open;

        public NetworkVariable<bool> Open => open;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            open.OnValueChanged += HandleOpenChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            
            open = new NetworkVariable<bool>(true);
        }

        private void HandleOpenChanged(bool open, bool oldOpen)
        {

        }
    }
}