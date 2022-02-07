using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(DoorInteractableState))]
    public class ClientDoorInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject doorSprite;
        
        private DoorInteractableState doorInteractableState;

        public override void Highlight()
        {
            highlight.SetActive(true);
        }

        public override void Unhighlight()
        {
            highlight.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            doorInteractableState.Open.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            doorSprite.SetActive(!open);
        }

        protected override void Awake()
        {
            base.Awake();
            doorInteractableState = GetComponent<DoorInteractableState>();
        }
    }
}