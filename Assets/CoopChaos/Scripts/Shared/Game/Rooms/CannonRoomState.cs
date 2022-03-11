using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class CannonRoomState : InteractableObjectStateBase
    {
        private ServerCannonRoom server;
        
        private NetworkVariable<bool> isBlocked;
        
        // loading in percent, 1.0f == loaded
        public NetworkVariable<float> BulletLoad { get; private set; }
        public NetworkVariable<float> Angle { get; private set; }
        
        public NetworkVariable<bool> IsBlocked { get; private set; }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            isBlocked.OnValueChanged += HandleIsBlockedChanged;
        }

        private int i = 0;
        
        [ServerRpc]
        public void ShootServerRpc()
        {
            Debug.Log("Shoot called" + ++i);
            server.Shoot();
        }
        
        [ServerRpc]
        public void SetAngleServerRpc(float angle)
        {
            Angle.Value = angle;
        }

        protected override void Awake()
        {
            base.Awake();
            
            isBlocked = new NetworkVariable<bool>(false);
            BulletLoad = new NetworkVariable<float>(1.0f);
            Angle = new NetworkVariable<float>(0.0f);
            
            server = GetComponent<ServerCannonRoom>();
        }
        
        private void HandleIsBlockedChanged(bool blocked, bool oldBlocked)
        {
            // TODO: Show or don't show Cannon Control
        }
    }
}
