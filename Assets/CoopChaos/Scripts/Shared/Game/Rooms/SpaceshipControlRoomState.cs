using Unity.Netcode;

namespace CoopChaos
{
    public class SpaceshipControlRoomState : InteractableObjectStateBase
    {
        private ServerSpaceshipControlRoom server;
        
        private NetworkVariable<bool> isBlocked;

        private NetworkVariable<float> verticalSlider;
        private NetworkVariable<float> horizontalSlider;
        
        public NetworkVariable<bool> IsBlocked => isBlocked;
        public NetworkVariable<float> VerticalSlider => verticalSlider;
        public NetworkVariable<float> HorizontalSlider => horizontalSlider;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            isBlocked.OnValueChanged += HandleIsBlockedChanged;
        }

        [ServerRpc]
        public void SetVerticalSliderServerRpc(float value)
        {
            verticalSlider.Value = value;
            server.SetVertical(value);
        }
        
        [ServerRpc]
        public void SetHorizontalSliderServerRpc(float value)
        {
            horizontalSlider.Value = value;
            server.SetHorizontal(value);
        }

        protected override void Awake()
        {
            base.Awake();
            
            isBlocked = new NetworkVariable<bool>(false);
            verticalSlider = new NetworkVariable<float>(0f);
            horizontalSlider = new NetworkVariable<float>(0.5f);

            server = GetComponent<ServerSpaceshipControlRoom>();
        }
        
        private void HandleIsBlockedChanged(bool blocked, bool oldBlocked)
        {
            // TODO: Show or don't show Spaceship Control
        }
    }
}
