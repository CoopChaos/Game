using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonInteractableState))]
    public class ClientCannonInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        private GameObject cannonControlMenu;
        private CannonInteractableState interactableState;
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

            interactableState.IsBlocked.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
        
        protected override void Awake()
        {
            base.Awake();
            interactableState = GetComponent<CannonInteractableState>();
            cannonControlMenu = GameObject.Find("CannonControlMenu");
            
            Assert.IsNotNull(interactableState);
            Assert.IsNotNull(cannonControlMenu);
            
        }
    }
}
