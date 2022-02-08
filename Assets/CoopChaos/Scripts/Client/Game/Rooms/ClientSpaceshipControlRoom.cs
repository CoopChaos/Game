using System;
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

        private GameObject spaceshipControlMenu;
        private SpaceshipControlRoomState state;
        
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

            state.IsBlocked.OnValueChanged += HandleOpenChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            state = GetComponent<SpaceshipControlRoomState>();
            spaceshipControlMenu = GameObject.Find("ControlMenu");
            
            Assert.IsNotNull(state);
            Assert.IsNotNull(spaceshipControlMenu);
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
