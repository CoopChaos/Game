using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(SpaceshipControlRoomState))]
    public class ClientSpaceshipControlRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        private GameObject spaceshipControlMenu;
        private SpaceshipControlRoomState spaceshipControlRoomState;
        public override void Highlight()
        {
            highlight.SetActive(true);
        }
        
        public override void Unhighlight()
        {
            highlight.SetActive(false);
            spaceshipControlMenu.SetActive(false);
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            spaceshipControlRoomState.IsBlocked.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
        
        protected override void Awake()
        {
            base.Awake();
            spaceshipControlRoomState = GetComponent<SpaceshipControlRoomState>();
            spaceshipControlMenu = GameObject.Find("SpaceshipControlMenu");
            
            Assert.IsNotNull(spaceshipControlRoomState);
            Assert.IsNotNull(spaceshipControlMenu);
            
        }
    }
}
