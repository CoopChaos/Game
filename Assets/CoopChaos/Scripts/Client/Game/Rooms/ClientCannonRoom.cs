using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonRoomState))]
    public class ClientCannonRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject cannonControlMenu;

        private CannonRoomState cannonRoomState;
        private bool highlighted = false;

        public override void Highlight()
        {
            highlighted = true;
            highlight.SetActive(true);
        }

        public override void Unhighlight()
        {
            highlighted = false;
            highlight.SetActive(false);
            cannonControlMenu.SetActive(false);
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            cannonRoomState.IsBlocked.OnValueChanged += HandleOpenChanged;

            cannonRoomState.InteractEvent += user =>
            {
                if (user == NetworkManager.Singleton.LocalClientId && highlighted)
                    cannonControlMenu.SetActive(!cannonControlMenu.activeSelf);
            };
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            cannonRoomState = GetComponent<CannonRoomState>();
            
            Assert.IsNotNull(cannonRoomState);
            
        }
    }
}
