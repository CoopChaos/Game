using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(SpaceshipControlInteractableState))]
    public class ClientSpaceshipControlInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        private GameObject spaceshipControlMenu;
        private SpaceshipControlInteractableState interactableState;
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

            interactableState.IsBlocked.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
        
        protected override void Awake()
        {
            base.Awake();
            interactableState = GetComponent<SpaceshipControlInteractableState>();
            spaceshipControlMenu = GameObject.Find("SpaceshipControlMenu");
            
            Assert.IsNotNull(interactableState);
            Assert.IsNotNull(spaceshipControlMenu);
            
        }
    }
}
