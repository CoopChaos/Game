using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonRoomState))]
    public class ClientCannonRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        private GameObject cannonControlMenu;
        private CannonRoomState cannonRoomState;
        public override void Highlight()
        {
            highlight.SetActive(true);
        }
        
        public override void Unhighlight()
        {
            highlight.SetActive(false);
            cannonControlMenu.SetActive(false);
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            cannonRoomState.IsBlocked.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
        
        protected override void Awake()
        {
            base.Awake();
            cannonRoomState = GetComponent<CannonRoomState>();
            cannonControlMenu = GameObject.Find("CannonControlMenu");
            
            Assert.IsNotNull(cannonRoomState);
            Assert.IsNotNull(cannonControlMenu);
            
        }
    }
}