using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    [RequireComponent(typeof(DeviceInteractableState), typeof(InteractableObjectBehaviour))]
    public class ClientDeviceInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject deviceSprite;
        [SerializeField] private GameObject highlight;
        
        private DeviceInteractableState deviceInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        private void HandleClaimChanged(bool open, bool oldOpen)
        {
            deviceSprite.SetActive(!open);
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
            
            deviceInteractableState = GetComponent<DeviceInteractableState>();
            
            interactable.OnHighlight += HandleOnHighlight;
            interactable.OnUnhighlight += HandleOnUnhighlight;
        }
    }
}