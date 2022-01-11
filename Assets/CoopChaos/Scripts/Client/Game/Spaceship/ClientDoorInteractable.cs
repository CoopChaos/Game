using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Yame
{
    [RequireComponent(typeof(DoorInteractableState), typeof(InteractableObjectBehaviour))]
    public class ClientDoorInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject doorSprite;
        
        private DoorInteractableState doorInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            doorInteractableState.Open.OnValueChanged += HandleOpenChanged;
        }

        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            doorSprite.SetActive(!open);
        }
        
        private void HandleOnHighlight()
        {
            highlight.SetActive(true);
        }
        
        private void HandleOnUnhighlight()
        {
            highlight.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            
            doorInteractableState = GetComponent<DoorInteractableState>();
            
            interactable.OnHighlight += HandleOnHighlight;
            interactable.OnUnhighlight += HandleOnUnhighlight;
        }
    }
}