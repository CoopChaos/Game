using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos.CoopChaos.Scripts.Client.Game.Spaceship
{
    [RequireComponent(typeof(RadarInteractableState))]
    public class ClientRadarInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        private GameObject radarMenu;
        private RadarInteractableState interactableState;
        public override void Highlight()
        {
            highlight.SetActive(true);
        }
        
        public override void Unhighlight()
        {
            highlight.SetActive(false);
            radarMenu.SetActive(false);
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
            interactableState = GetComponent<RadarInteractableState>();
            radarMenu = GameObject.Find("RadarMenu");
            
            Assert.IsNotNull(interactableState);
            Assert.IsNotNull(radarMenu);
            
        }
    }
}