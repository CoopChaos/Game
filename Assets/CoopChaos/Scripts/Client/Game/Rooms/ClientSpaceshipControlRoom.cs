using System;
using TMPro;
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
        

        [SerializeField] private TextMeshProUGUI SpeedometerVertical;
        [SerializeField] private TextMeshProUGUI SpeedometerHorizontal;
        [SerializeField] private GameObject spaceshipControlMenu;
        private SpaceshipControlRoomState spaceshipControlRoomState;
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

            spaceshipControlRoomState.IsBlocked.OnValueChanged += HandleOpenChanged;

            spaceshipControlRoomState.InteractEvent += user =>
            {
                if(user == NetworkManager.Singleton.LocalClientId && highlighted)
                    spaceshipControlMenu.SetActive(!spaceshipControlMenu.activeSelf);
            };

            spaceshipControlRoomState.VerticalVelocity.OnValueChanged += HandleVerticalVelocityChanged;
            spaceshipControlRoomState.HorizontalVelocity.OnValueChanged += HandleHorizontalVelocityChanged;
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
            
        }

        private void HandleVerticalVelocityChanged(float verticalVelocity, float oldVerticalVelocity) => SpeedometerVertical.text = verticalVelocity.ToString();
        private void HandleHorizontalVelocityChanged(float horizontalVelocity, float oldHorizontalVelocity) => SpeedometerHorizontal.text = horizontalVelocity.ToString();        
        protected override void Awake()
        {
            base.Awake();
            
            spaceshipControlRoomState = GetComponent<SpaceshipControlRoomState>();
            
            Assert.IsNotNull(spaceshipControlRoomState);
        }

        private void Start()
        {
            spaceshipControlRoomState.InteractEvent += user =>
            {
                verticalSlider.value = spaceshipControlRoomState.VerticalSlider.Value;
                horizontalSlider.value = spaceshipControlRoomState.HorizontalSlider.Value;
            };
            
            verticalSlider.onValueChanged.AddListener(v => spaceshipControlRoomState.SetVerticalSliderServerRpc(v));
            horizontalSlider.onValueChanged.AddListener(v => spaceshipControlRoomState.SetHorizontalSliderServerRpc(v));
        }
    }
}
