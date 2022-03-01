using Unity.Netcode;

namespace CoopChaos
{
    public class SpaceshipControlRoomState : InteractableObjectStateBase
    {
        private ServerSpaceshipControlRoom server;
        
        public NetworkVariable<bool> IsBlocked { get; private set; }
        public NetworkVariable<float> VerticalSlider { get; private set; }
        public NetworkVariable<float> HorizontalSlider { get; private set; }
        public NetworkVariable<float> VerticalVelocity { get; private set; }
        public NetworkVariable<float> HorizontalVelocity { get; private set; }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            IsBlocked.OnValueChanged += HandleIsBlockedChanged;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetVerticalSliderServerRpc(float value)
        {
            VerticalSlider.Value = value;
            server.SetVertical(value);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SetHorizontalSliderServerRpc(float value)
        {
            HorizontalSlider.Value = value;
            server.SetHorizontal(value);
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetVerticalVelocityServerRpc(float value) => VerticalVelocity.Value = value;

        [ServerRpc(RequireOwnership = false)]
        public void SetHorizontalVelocityServerRpc(float value) => HorizontalVelocity.Value = value;

        protected override void Awake()
        {
            base.Awake();
            
            IsBlocked = new NetworkVariable<bool>(false);
            VerticalSlider = new NetworkVariable<float>(0f);
            HorizontalSlider = new NetworkVariable<float>(0.5f);

            server = GetComponent<ServerSpaceshipControlRoom>();
        }
        
        private void HandleIsBlockedChanged(bool blocked, bool oldBlocked)
        {
            // TODO: Show or don't show Spaceship Control
        }
    }
}
