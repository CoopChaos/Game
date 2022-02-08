using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace CoopChaos
{
    [RequireComponent(typeof(SpaceshipControlRoomState))]
    public class ClientSpaceshipControlRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        
        [SerializeField] private ElasticSlider verticalSlider;
        [SerializeField] private ElasticSlider horizontalSlider;

        [SerializeField] private GameObject spaceshipControlMenu;
        private SpaceshipControlRoomState state;
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
            spaceshipControlMenu.SetActive(false);
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            state.IsBlocked.OnValueChanged += HandleOpenChanged;

            state.InteractEvent += user =>
            {
                if(user == NetworkManager.Singleton.LocalClientId && highlighted)
                    spaceshipControlMenu.SetActive(!spaceshipControlMenu.activeSelf);
            };
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            state = GetComponent<SpaceshipControlRoomState>();
            
            Assert.IsNotNull(state);
        }

        private void Start()
        {
            state.InteractEvent += user =>
            {
                verticalSlider.value = state.VerticalSlider.Value;
                horizontalSlider.value = state.HorizontalSlider.Value;
            };
            
            verticalSlider.onValueChanged.AddListener(v => state.SetVerticalSliderServerRpc(v));
            horizontalSlider.onValueChanged.AddListener(v => state.SetHorizontalSliderServerRpc(v));
        }
    }
}
