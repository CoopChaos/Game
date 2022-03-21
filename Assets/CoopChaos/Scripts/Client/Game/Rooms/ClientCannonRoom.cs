using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonRoomState))]
    public class ClientCannonRoom : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject cannonControlMenu;

        [SerializeField] private Button shootButton;

        [SerializeField] private Color normalColor;
        [SerializeField] private Color loadedColor;
        [SerializeField] private RectTransform bulletLoadingBar;

        [SerializeField] private ElasticSlider slider;

        private CannonRoomState state;
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
            cannonControlMenu.SetActive(false);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            state.IsBlocked.OnValueChanged += HandleOpenChanged;

            state.InteractEvent += user =>
            {
                if (user == NetworkManager.Singleton.LocalClientId && highlighted)
                    cannonControlMenu.SetActive(!cannonControlMenu.activeSelf);
            };

            state.BulletLoad.OnValueChanged += (ov, v) =>
            {
                var ls = bulletLoadingBar.localScale;
                ls.y = v;
                bulletLoadingBar.localScale = ls;

                if (v == 1f)
                {
                    shootButton.interactable = true;
                    bulletLoadingBar.GetComponent<Image>().color = loadedColor;   
                }
                else
                {
                    shootButton.interactable = false;
                    bulletLoadingBar.GetComponent<Image>().color = normalColor;
                }
            };
            
            shootButton.onClick.AddListener(() =>
            {
                Debug.Log("Shootc");
                state.ShootServerRpc();
            });
            
            
            slider.onValueChanged.AddListener(v =>
            {
                state.SetAngleServerRpc(v * 360f - 180f);
            });
        }
        
        private void HandleOpenChanged(bool open, bool oldOpen)
        {
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            state = GetComponent<CannonRoomState>();
            bulletLoadingBar.GetComponent<Image>().color = loadedColor;

            Assert.IsNotNull(state);
        }
    }
}
